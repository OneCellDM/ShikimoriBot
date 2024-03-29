﻿using Newtonsoft.Json;

namespace BotShared.ShikimoriSharp.Classes
{
    public class Activity
    {
        [JsonProperty("name")] public long[] Name { get; set; }
        [JsonProperty("value")] public long? Value { get; set; }
    }
}