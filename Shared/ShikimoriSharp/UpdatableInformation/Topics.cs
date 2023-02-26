using System.Threading.Tasks;

using BotShared.ShikimoriSharp.AdditionalRequests;
using BotShared.ShikimoriSharp.Bases;
using BotShared.ShikimoriSharp.Settings;

namespace BotShared.ShikimoriSharp.UpdatableInformation
{
    public class Topics : ApiBase
    {
        public Topics(ApiClient apiClient) : base(Enums.Version.v1, apiClient)
        {
        }

        public async Task<Topic[]> GetTopics(TopicSettings settings = null, AccessToken personalInformation = null)
        {
            return await Request<Topic[], TopicSettings>("topics", settings, personalInformation);
        }

        public async Task<ExtendedLightTopic[]> GetUpdates(BasicSettings settings = null)
        {
            return await Request<ExtendedLightTopic[], BasicSettings>("topics/updates", settings);
        }

        public async Task<Topic> GetTopics(int id, AccessToken personalInformation = null)
        {
            return await Request<Topic>($"topics/{id}", personalInformation);
        }

        public async Task<Topic> CreateTopic(CreateTopicSettings settings, AccessToken personalInformation)
        {
            Requires(personalInformation, new[] { "topics" });
            return await SendJson<Topic>("topics", settings, personalInformation);
        }

        public async Task DeleteTopic(int id, AccessToken personalInformation)
        {
            Requires(personalInformation, new[] { "topics" });
            await NoResponseRequest($"topics/{id}", personalInformation);
        }
    }
}