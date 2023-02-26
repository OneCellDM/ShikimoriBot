using BotShared.ShikimoriSharp.Bases;

using Newtonsoft.Json;

namespace BotShared.ShikimoriSharp.Classes
{
    public class Manga : AnimeMangaBase
    {
        [JsonProperty("volumes")] public long Volumes { get; set; }
        [JsonProperty("chapters")] public long Chapters { get; set; }
    }

    public class MangaID : AnimeMangaIdBase
    {
        [JsonProperty("volumes")] public long Volumes { get; set; }
        [JsonProperty("chapters")] public long Chapters { get; set; }
        [JsonProperty("publishers")] public Publisher[] Publishers { get; set; }
    }
}