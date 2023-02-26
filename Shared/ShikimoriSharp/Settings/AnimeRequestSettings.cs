#nullable enable
using BotShared.ShikimoriSharp.Enums;
using BotShared.ShikimoriSharp.Settings;

namespace BotShared.ShikimoriSharp.Settings
{
    public class AnimeRequestSettings : MangaAnimeRequestSettingsBase
    {
        public string? rating;
        public Duration? duration;
        public int[]? studio;
    }
}