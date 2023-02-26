using System;
using System.Threading.Tasks;

using BotShared.ShikimoriSharp.Bases;
using BotShared.ShikimoriSharp.Settings;

using Newtonsoft.Json;

namespace BotShared.ShikimoriSharp.Information
{
    public class Achievements : ApiBase
    {
        public Achievements(ApiClient apiClient) : base(Enums.Version.v1, apiClient)
        {
        }

        public async Task<Achievement[]> GetAchievements(AchievementsSettings settings)
        {
            return await Request<Achievement[], AchievementsSettings>("achievements", settings);
        }

        public class Achievement
        {
            [JsonProperty("id")] public long? Id { get; set; }

            [JsonProperty("neko_id")] public string NekoId { get; set; }

            [JsonProperty("level")] public long? Level { get; set; }

            [JsonProperty("progress")] public long? Progress { get; set; }

            [JsonProperty("user_id")] public long? UserId { get; set; }

            [JsonProperty("created_at")] public DateTimeOffset? CreatedAt { get; set; }

            [JsonProperty("updated_at")] public DateTimeOffset? UpdatedAt { get; set; }
        }
    }
}