using BotShared.ShikimoriSharp.Bases;

using Newtonsoft.Json;

namespace BotShared.ShikimoriSharp.Classes
{
    public class Character
    {
        [JsonProperty("id")] public long Id { get; set; }
        [JsonProperty("name")] public string Name { get; set; }
        [JsonProperty("russian")] public string Russian { get; set; }
        [JsonProperty("image")] public Image Image { get; set; }
        [JsonProperty("url")] public string Url { get; set; }
    }
}