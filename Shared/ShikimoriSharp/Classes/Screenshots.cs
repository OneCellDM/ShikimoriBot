﻿using Newtonsoft.Json;

namespace BotShared.ShikimoriSharp.Classes
{
    public class Screenshots
    {
        [JsonProperty("original")] public string Original { get; set; }
        [JsonProperty("preview")] public string Preview { get; set; }
    }
}