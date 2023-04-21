using BotShared;
var anime = await ShikimoriApiHandler.GetRandomAnime();
Console.WriteLine(anime.Item2);