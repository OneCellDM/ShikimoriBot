using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;

using Telegram.Bot.Types.ReplyMarkups;
using File = System.IO.File;
using BotShared.ShikimoriSharp.Classes;
using BotShared;

namespace TgShikimoriBot
{

    public partial class Program
    {
        public class BotClient : IBot <TelegramBotClient> 
        {

            const string TokenFileName = "Token.txt";
            public TelegramBotClient BotApi { get; set; }
            public CancellationTokenSource cts;

            public event IBot.LongPoll MessageLongPollEvent;

            private ReceiverOptions receiverOptions { get; set; }
            public async Task Init()
            {

                string token = string.Empty;

                while (string.IsNullOrEmpty(token))
                {
                    if (File.Exists(TokenFileName))
                    {
                        token = File.ReadAllText(TokenFileName).Trim();
                    }

                    if (string.IsNullOrEmpty(token))
                    {
                        Console.Write("Input Token >");
                        token = Console.ReadLine()?.Trim();
                        File.WriteAllText(TokenFileName, token);
                    }
                }

                BotApi = new TelegramBotClient(token);

                receiverOptions = new()
                {
                    AllowedUpdates = new[] { UpdateType.Message, UpdateType.CallbackQuery } 
                };

               
            }

            public async Task SendAnimeDetailAsync(long chatId,long animeId)
            {
               
                    var info = await ShikimoriApiHandler.GetAnimeInfo(animeId);

                    await BotApi.SendTextMessageAsync(chatId, info.Item1);

                    if (info.Item2 != null && info.Item2.Count > 0)
                    {

                        var photos = info.Item2?.Select(uri => new InputMediaPhoto(uri));
                        if (photos != null && photos.Count() > 0)
                        {
                            await BotApi.SendTextMessageAsync(chatId, $"{BotConstans.SendScreenShots}");
                            await BotApi.SendMediaGroupAsync(chatId, photos);
                        }
                    }
                
                

            }

            public async Task SendMangaDetailAsync(long chatId, long mangaId)
            {
                var manga = await ShikimoriApiHandler.GetMangaInfo(mangaId);

                if (manga.poster != null)
                {
                    await BotApi.SendPhotoAsync(chatId, caption: manga.text, photo: manga.poster);
                }
                else await BotApi.SendTextMessageAsync(chatId, manga.text);
            }

            public async Task SendRandomAnimeAsync(long chatId)
            {
                var anime = await ShikimoriApiHandler.GetRandomAnime();
                await BotApi.SendTextMessageAsync(chatId, anime.Item2,
                    replyMarkup: 
                    new InlineKeyboardMarkup(InlineKeyboardButton.WithCallbackData(
                        BotConstans.GetAnime.Title, 
                        $"{BotConstans.GetAnime.Command} {anime.Item1}")));
            }

            public async Task SendRandomMangaAsync(long chatId)
            {
                var manga = await ShikimoriApiHandler.GetRandomManga();
                await BotApi.SendTextMessageAsync(chatId, manga.Item2,
                    replyMarkup: new InlineKeyboardMarkup(InlineKeyboardButton.WithCallbackData(
                        BotConstans.GetManga.Title, 
                        $"{BotConstans.GetManga.Command} {manga.Item1}")));
            }
            public async Task SendDefaultMessageAsync(long chatId)
            {
                InlineKeyboardMarkup inlineKeyboardButtons = new InlineKeyboardMarkup(new[] 
                {
                    InlineKeyboardButton.WithCallbackData(BotConstans.RandomAnime.Title, BotConstans.RandomAnime.Command),
                    InlineKeyboardButton.WithCallbackData(BotConstans.RandomManga.Title, BotConstans.RandomManga.Command),
                    InlineKeyboardButton.WithCallbackData(BotConstans.Help.Title, BotConstans.Help.Command),
                });

                await BotApi.SendTextMessageAsync(chatId,BotConstans.InputAction,replyMarkup:inlineKeyboardButtons);
            }

            public  async Task<bool> Start()
            {
                try
                {
                    cts = new CancellationTokenSource();
                    BotApi.StartReceiving(
                      updateHandler: HandleUpdateAsync,
                      pollingErrorHandler: HandlePollingErrorAsync,
                      receiverOptions: receiverOptions,
                    cancellationToken: cts.Token);


                    var me = await BotApi.GetMeAsync();
                   
                    Console.WriteLine("Бот запущен");
                    Console.WriteLine($"Start listening for @{me.Username}");
                    return true;

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Ошибка запуска бота, проверьте правильность токена");
                    File.Delete(TokenFileName);
                    return false;
     
                }
            }

            public  async Task<bool> Stop()
            {
                try
                {
                   
                    if (BotApi != null)
                    {
                        cts.Cancel();
                        await BotApi.CloseAsync();
                    }
                    
                    return true;
                }
                catch(Exception ex)
                {
                    return false;
                }
                
            }

            private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
            {

                long? chatId = 0;
                string? message = string.Empty;
                string[]? cmdArgs = null;

                if (update.CallbackQuery != null)
                {
                    chatId = update.CallbackQuery?.Message?.Chat.Id;
                    message = update.CallbackQuery?.Data;
                }

                if (update.Message != null && update.Message.Text != null)
                {
                    chatId = update.Message.Chat.Id;
                    message = update.Message.Text;
                }

                MessageLongPollEvent?.Invoke(chatId, message);

            }

            private Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
            {
                var ErrorMessage = exception switch
                {
                    ApiRequestException apiRequestException
                        => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                    _ => exception.ToString()
                };

                Console.WriteLine("ErrorMessage:\n" + ErrorMessage);
                return Task.CompletedTask;
            }

            public async Task SendHelpMessageAsync(long chatId)
            {
                var text = BotConstans.GetCommandsString();
                await BotApi.SendTextMessageAsync(chatId, text);
            }
        }

    }
}



