﻿using Newtonsoft.Json;

namespace BotShared.ShikimoriSharp.Classes
{
    public class Dialog
    {
        [JsonProperty("target_user")] public User TargetUser { get; set; }
        [JsonProperty("message")] public MessageContent Message { get; set; }
    }
}