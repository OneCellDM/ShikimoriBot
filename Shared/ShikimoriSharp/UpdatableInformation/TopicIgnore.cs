using System.Threading.Tasks;

using BotShared.ShikimoriSharp;
using BotShared.ShikimoriSharp.Bases;

namespace BotShared.ShikimoriSharp.UpdatableInformation
{
    public class TopicIgnores : ApiBase
    {
        public TopicIgnores(ApiClient apiClient) : base(Enums.Version.v2, apiClient)
        {
        }

        public async Task Ignore(int id, AccessToken personalInformation)
        {
            Requires(personalInformation, new[] { "ignores" });
            await NoResponseRequest($"topics/{id}/ignore", personalInformation);
        }

        public async Task UnIgnore(int id, AccessToken personalInformation)
        {
            Requires(personalInformation, new[] { "ignores" });
            await NoResponseRequest($"topics/{id}/ignore", personalInformation, method: "DELETE");
        }
    }
}