using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;

using Telegram.Bot.Types.ReplyMarkups;
using File = System.IO.File;

namespace TgShikimoriBot
{
    public class Program
    {
        const string TokenFileName = "Token.txt";
        static CancellationTokenSource cts = new();

        public static TelegramBotClient botClient;

        static InlineKeyboardMarkup Mainkeyboard = new InlineKeyboardMarkup(new[]
        {
            InlineKeyboardButton.WithCallbackData("Случайное аниме","/randomanime"),
            InlineKeyboardButton.WithCallbackData("Случайная манга","/randommanga"),
        });

        public static async Task Main(string[] args)
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
            

            botClient = new TelegramBotClient(token);
            ReceiverOptions receiverOptions = new()
            {
                AllowedUpdates = new[] { UpdateType.Message, UpdateType.CallbackQuery } // receive all update types
            };
            botClient.StartReceiving(
              updateHandler: HandleUpdateAsync,
              pollingErrorHandler: HandlePollingErrorAsync,
              receiverOptions: receiverOptions,
              cancellationToken: cts.Token);
            var me = await botClient.GetMeAsync();

            Console.WriteLine($"Start listening for @{me.Username}");
            Console.ReadLine();

            // Send cancellation request to stop bot
            cts.Cancel();
        }
        
        static string[] SplitArgs(string text)
        {
           return  text.Trim().Split(" ").Where(x => x.Length > 0).ToArray();
        }
        static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
           
            long chatId = 0;
            string message = string.Empty;
            string[] cmdArgs = null;
            if (update.CallbackQuery != null)
            {
                chatId = update.CallbackQuery.Message.Chat.Id;
                message = update.CallbackQuery.Data;
               
            }

            if (update.Message != null && update.Message.Text != null) 
            {
                chatId = update.Message.Chat.Id;
                message = update.Message.Text;
            }
           
            cmdArgs = SplitArgs(message);


            if (cmdArgs == null || cmdArgs.Length == 0 ) return;

            try
            {
                switch (cmdArgs[0])
                {

                    case "/randomanime":
                        {
                            await botClient.SendTextMessageAsync(chatId, await GetRandomAnime());
                            break;
                        }
                    case "/randommanga":
                        {
                            await botClient.SendTextMessageAsync(chatId, await GetRandomManga());
                            break;
                        }
                }
                await botClient.SendTextMessageAsync(chatId,"Выберите действие", parseMode:ParseMode.MarkdownV2, replyMarkup: Mainkeyboard);
            }
            catch(Exception ex)
            {
                Console.WriteLine("Ошибка:"+ex.Message);
                Console.WriteLine(ex);
            }
        }

        static Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException
                    => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            Console.WriteLine(ErrorMessage);
            return Task.CompletedTask;
        }
        
        public static async Task<string> GetRandomManga()
        {
            return await Task.Run<string>(async () =>
            {
                try
                {
                    var mangaArray = await ShikimoriApiHandler.ApiClient.Mangas.GetBySearch(new ShikimoriSharp.Settings.MangaRequestSettings()
                    {
                        limit = 1,
                        order = "random"
                    });
                    if (mangaArray.Length > 0)
                    {
                        var manga = mangaArray.First();
                        return $"Случайная  манга\n\n" +
                        $"{manga.Name} ({manga.Russian})\n" +
                        $"Дата выхода:{manga?.AiredOn?.ToString("d")}\n" +
                        $"Статус:{manga.Status}\n" +
                        $"Тип:{manga.Kind}\n" +
                        $"Оценка:{manga.Score}\n" +
                        $"Тома:{manga.Chapters}\n" +
                        $"Главы:{manga.Chapters}\n" +
                        $"Подробнее: https://shikimori.one{manga.Url}";

                    }
                    else return "Список манги пуст";
                }
                catch(Exception ex)
                {
                    return "Произошла проблема при получении манги";
                }
            });
        }
        public static async Task<string> GetRandomAnime()
        {
           
            return await Task.Run<string>(async () =>
            {
                try
                {
                    var animeArray = await ShikimoriApiHandler.ApiClient.Animes.GetAnime(new ShikimoriSharp.Settings.AnimeRequestSettings()
                    {
                       limit = 1,
                       order = "random"
                    });


                    if (animeArray.Length > 0)
                    {
                        
                        var anime = animeArray.First();
                        
                        return $"Случайное аниме\n\n" +
                        $"{anime.Name} ({anime.Russian})\n" +
                        $"Дата выхода:{anime?.AiredOn?.ToString("d")}\n" +
                        $"Статус:{anime.Status}\n" +
                        $"Тип:{anime.Kind}\n" +
                        $"Оценка:{anime.Score}\n" +
                        $"Эпизоды:{anime.Episodes}/{anime.EpisodesAired}\n" +
                        $"Подробнее: https://shikimori.one{anime.Url}"; 
                        

                    }
                    else 
                        return "Список аниме пуст";
                }
                catch(Exception)
                {
                   return "Произошла проблема при получении  аниме";
                }
               
            });
           
        }
    }
}



