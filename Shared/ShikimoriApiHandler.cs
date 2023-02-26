using BotShared.ShikimoriSharp;

using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotShared
{
    public static class ShikimoriApiHandler
    {
        const string domain = "https://shikimori.one";

        class Logger : ILogger
        {
            public IDisposable BeginScope<TState>(TState state)
            {
                throw new NotImplementedException();
            }

            public bool IsEnabled(LogLevel logLevel)
            {
                return true;
            }



            public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
            {
                Debug.WriteLine(state);
            }
        }




        private static bool _IsInit = false;
        private static ShikimoriClient? _ApiClient;

        public static ShikimoriClient? ApiClient { get; set; }

        static ShikimoriApiHandler()
        {
            ApiClient = new ShikimoriClient(

                   new Logger(),
                   new("ShikikomoriOneCellDMApp", "To5K_6HfyRpSp_ZpOap8WReJFkjv7O3_Wj7dKl4_X90", "s-gUpfM5icFV18PI0ljmM0rpxBPiMqhNjC113C_0hFo")

                );


        }

        public static async Task<(string text, string? poster)> GetMangaInfo(long id)
        {
            try
            {
                var manga = await ApiClient.Mangas.GetById(id);

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
                        $"Издатели: {publishes ?? "Неизвестно"}\n" +
                        $"Подробнее: {domain}{manga.Url}";

                string? poster = null;
                if (string.IsNullOrEmpty(manga.Image?.Original) == false)
                {
                    poster = $"{domain}{manga.Image.Original}";
                }
                return (text, poster);

            }
            catch (Exception ex)
            {
                return ("Произошла ошибка при получении информации о манге", null);
            }
        }
        public static async Task<(string, List<string>?)> GetAnimeInfo(long id)
        {
            return await Task.Run(async () =>
            {
                try
                {
                    var anime = await ApiClient.Animes.GetAnime(id);

                    var genres = anime.Genres.Aggregate("", (cur, next) => cur + ", " + next.Russian);
                    var text = $"Подробности об аниме: \n{anime.Name} {anime.Russian} (id: {anime.Id})\n" +
                        $"Дата выхода: {anime?.AiredOn?.ToString("d")}\n" +
                        $"Статус: {anime.Status}\n" +
                        $"Тип: {anime.Kind}\n" +
                        $"Жанры: {genres ?? "Неизвестно"}\n" +
                        $"Оценка: {anime.Score}\n" +
                        $"Рейтинг: {anime.Rating ?? "Неизвестно"}\n" +
                        $"Эпизоды: {anime.EpisodesAired}/{anime.Episodes}\n" +
                        $"Продолжительность: {anime.Duration}\n" +
                        $"Описание: {anime?.Description ?? "Неизвестно"}\n";

                    List<string> screens = new List<string>();
                    foreach (var item in anime.Screens)
                    {
                        screens.Add($"{domain}{item.Original}");
                    }
                    return (text, screens);


                }
                catch (Exception ex)
                {
                    return ("Произошла проблема при получении информации об аниме", null);
                }

            });
        }
        public static async Task<(long?, string)> GetRandomManga()
        {
            return await Task.Run<(long?, string)>(async () =>
            {
                try
                {
                    var mangaArray = await ApiClient.Mangas.GetBySearch(new ShikimoriSharp.Settings.MangaRequestSettings()
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
                    else return (-1, "Список манги пуст");
                }
                catch (Exception ex)
                {
                    return (-1, "Произошла проблема при получении манги");
                }
            });
        }
        public static async Task<(long?, string)> GetRandomAnime()
        {

            return await Task.Run<(long?, string)>(async () =>
            {
                try
                {
                    var animeArray = await ApiClient.Animes.GetAnime(new ShikimoriSharp.Settings.AnimeRequestSettings()
                    {
                        limit = 1,
                        order = "random"
                    });


                    if (animeArray.Length > 0)
                    {

                        var anime = animeArray.First();

                        return (anime.Id, $"Случайное аниме\n\n" +
                        $"{anime.Name} {anime.Russian} (id: {anime.Id})\n" +
                        $"Дата выхода:{anime?.AiredOn?.ToString("d")}\n" +
                        $"Статус:{anime.Status}\n" +
                        $"Тип:{anime.Kind}\n" +
                        $"Оценка:{anime.Score}\n" +
                        $"Эпизоды:{anime.EpisodesAired}/{anime.Episodes}\n" +
                        $"Подробнее:{domain}{anime.Url}");


                    }
                    else
                        return (-1, "Список аниме пуст");
                }
                catch (Exception)
                {
                    return (-1, "Произошла проблема при получении  аниме");
                }

            });

        }
    }
}
