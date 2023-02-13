#nullable enable
using ShikimoriSharp.Enums;

namespace ShikimoriSharp.Settings
{
    public class MangaAnimeRequestSettingsBase : BasicSettings
    {
        public bool? censored;
        public string? exclude_ids;
        public string? franchise;
        public string? genre;
        public string? ids;
        public string? kind;
        public MyList? mylist;
        public string? order;
        public int? score;
        public string? search;
        public string? season;
        public string? status;
    }
}