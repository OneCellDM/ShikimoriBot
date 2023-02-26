using Newtonsoft.Json;

namespace BotShared.ShikimoriSharp.Classes
{
    public class Publisher
    {
        [JsonProperty("id")] public long Id { get; set; }
        [JsonProperty("name")] public string Name { get; set; }
    }
}