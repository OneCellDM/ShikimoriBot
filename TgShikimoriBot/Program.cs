using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;

using Telegram.Bot.Types.ReplyMarkups;
using File = System.IO.File;
using ShikimoriSharp.Classes;
using ShikimoriSharp.Information;
using System.Xml.Linq;
using System;

namespace TgShikimoriBot
{
    public class Program
    {
        const string domain = "https://shikimori.one";
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
            try
            {
                botClient.StartReceiving(
                  updateHandler: HandleUpdateAsync,
                  pollingErrorHandler: HandlePollingErrorAsync,
                  receiverOptions: receiverOptions,
                  cancellationToken: cts.Token);
                var me = await botClient.GetMeAsync();


                Console.WriteLine($"Start listening for @{me.Username}");
                Console.ReadLine();
            }
            catch(Exception ex)
            {
                Console.WriteLine("Ошибка инициализации бота, проверьте правильность токена");
                File.Delete(TokenFileName);
                
            }


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

                            var anime = await GetRandomAnime();
                            await botClient.SendTextMessageAsync(chatId, anime.Item2, 
                                replyMarkup: new InlineKeyboardMarkup(InlineKeyboardButton.WithCallbackData("Подробнее", $"/anime {anime.Item1}")));
                            
                            break;
                        }
                    case "/randommanga":
                        {
                            var manga = await GetRandomManga();
                            await botClient.SendTextMessageAsync(chatId, manga.Item2, 
                                replyMarkup: new InlineKeyboardMarkup(InlineKeyboardButton.WithCallbackData("Подробнее", $"/manga {manga.Item1}")));
                            break;
                        }
                    case "/manga":
                        {
                            bool ok = false;
                            if(cmdArgs.Length > 1)
                            {
                                if (long.TryParse(cmdArgs[1], out long id))
                                {
                                    var manga = await GetMangaInfo(id);

                                    if (manga.poster != null) {
                                        await botClient.SendPhotoAsync(chatId,caption:manga.text, photo:manga.poster);
                                    }
                                    else await botClient.SendTextMessageAsync(chatId, manga.text);

                                    ok = true;
                                }

                            }
                            if(ok == false)
                                await botClient.SendTextMessageAsync(chatId, "Введите числовой ID манги на сайте");
                            break;
                        }
                    case "/anime":
                        {
                            bool ok = false;
                            if (cmdArgs.Length > 1)
                            {

                                if (long.TryParse(cmdArgs[1], out long id))
                                {
                                    var info = await GetAnimeInfo(id);

                                    await botClient.SendTextMessageAsync(chatId, info.Item1);

                                    if (info.Item2 != null && info.Item2.Count > 0)
                                    {
                                      
                                        var photos = info.Item2?.Select(uri => new InputMediaPhoto(uri));
                                        if (photos != null && photos.Count() > 0)
                                        {
                                            await botClient.SendTextMessageAsync(chatId, "Отправка скриншотов");
                                            await botClient.SendMediaGroupAsync(chatId, photos);
                                        }
                                        

                                    }
                                    ok = true;
                                }
                              
                            }
                           if(ok == false)
                                await botClient.SendTextMessageAsync(chatId, "Введите числовой ID аниме на сайте");
                            
                            break;
                        }
                }
                await botClient.SendTextMessageAsync(chatId, "Выберите действие", parseMode: ParseMode.MarkdownV2, replyMarkup: Mainkeyboard);

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
        public static async Task<(string text, string? poster)> GetMangaInfo(long id)
        {
            try
            {
                var manga = await ShikimoriApiHandler.ApiClient.Mangas.GetById(id);

                var genres = manga.Genres.Aggregate("", (cur, next) => cur + ", " + next.Russian);

                var publishes = manga.Publishers?.Aggregate("", (current, next) => current + ", " + next.Name);

                var text = $"Подробности о манге: \n{manga.Name} {manga.Russian} (id: {manga.Id})\n" +
                        $"Дата выхода:{manga?.AiredOn?.ToString("d")}\n" +
                        $"Статус:{manga.Status}\n" +
                        $"Тип:{manga.Kind}\n" +
                        $"Жанры: {genres ?? "Неизвестно"}\n" +
                        $"Оценка:{manga.Score}\n" +                     
                        $"Главы:{manga.Chapters}\n" +
                        $"Тома:{manga.Volumes}\n" +
                        $"Издатели: {publishes??"Неизвестно"}\n"+
                        $"Подробнее: {domain}{manga.Url}" ;

                string? poster = null;
                if (string.IsNullOrEmpty(manga.Image?.Original) == false)
                {
                    poster = $"{domain}{manga.Image.Original}";
                }
                return (text, poster);
                
            }
            catch(Exception ex) 
            {
                return ("Произошла ошибка при получении информации о манге",null);
            }
        }
        public static async Task<(string, List<string>?)> GetAnimeInfo(long id)
        {
            return await Task.Run(async () =>
            {
                try
                {
                    var anime = await ShikimoriApiHandler.ApiClient.Animes.GetAnime(id);

                    var genres = anime.Genres.Aggregate("", (cur, next) => cur + ", " + next.Russian);
                    var text = $"Подробности об аниме: \n{anime.Name} {anime.Russian} (id: {anime.Id})\n" +
                        $"Дата выхода: {anime?.AiredOn?.ToString("d")}\n" +
                        $"Статус: {anime.Status}\n" +
                        $"Тип: {anime.Kind}\n" +
                        $"Жанры: {genres??"Неизвестно"}\n" +
                        $"Оценка: {anime.Score}\n" +
                        $"Рейтинг: {anime.Rating??"Неизвестно"}\n" +
                        $"Эпизоды: {anime.EpisodesAired}/{anime.Episodes}\n" +
                        $"Продолжительность: {anime.Duration}\n" +
                        $"Описание: {anime?.Description??"Неизвестно"}\n";

                    List<string> screens = new List<string>();
                    foreach(var item in anime.Screens) 
                    {
                        screens.Add($"{domain}{item.Original}");
                    }
                    return (text, screens);
                    
                       
                }
                catch(Exception ex)
                {
                    return ("Произошла проблема при получении информации об аниме",null);
                }
               
            });
        }
        public static async Task<(long?, string)> GetRandomManga()
        {
            return await Task.Run < (long?,string) > (async () =>
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
                        
                        var str = $"Случайная  манга\n\n" +
                        $"{manga.Name} {manga.Russian} (id: {manga.Id})\n" +
                        $"Дата выхода:{manga?.AiredOn?.ToString("d")}\n" +
                        $"Статус:{manga.Status}\n" +
                        $"Тип:{manga.Kind}\n" +
                        $"Оценка:{manga.Score}\n" +
                        $"Главы:{manga.Chapters}\n" +
                        $"Тома:{manga.Volumes}\n" +
                        $"Подробнее: {domain}{manga.Url}";
                        return (manga.Id, str);

                    }
                    else return (-1,"Список манги пуст");
                }
                catch(Exception ex)
                {
                    return (-1,"Произошла проблема при получении манги");
                }
            });
        }
        public static async Task<(long?, string)>  GetRandomAnime()
        {
           
            return await Task.Run<(long?, string)>(async () =>
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
                        
                        return (anime.Id,$"Случайное аниме\n\n" +
                        $"{anime.Name} {anime.Russian} (id: {anime.Id})\n" +
                        $"Дата выхода:{anime?.AiredOn?.ToString("d")}\n" +
                        $"Статус:{anime.Status}\n" +
                        $"Тип:{anime.Kind}\n" +
                        $"Оценка:{anime.Score}\n" +
                        $"Эпизоды:{anime.EpisodesAired}/{anime.Episodes}\n" +
                        $"Подробнее:{domain}{anime.Url}"); 
                        

                    }
                    else 
                        return (-1,"Список аниме пуст");
                }
                catch(Exception)
                {
                   return (-1, "Произошла проблема при получении  аниме");
                }
               
            });
           
        }
    }
}



