#nullable enable
using ShikimoriSharp.Enums;

namespace ShikimoriSharp.Settings
{
    public class AnimeRequestSettings : MangaAnimeRequestSettingsBase
    {
        public string? rating;
        public Duration? duration;
        public int[]? studio;
    }
}