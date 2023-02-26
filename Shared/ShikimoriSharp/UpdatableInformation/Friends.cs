using System.Threading.Tasks;

using BotShared.ShikimoriSharp.Bases;

namespace BotShared.ShikimoriSharp.UpdatableInformation
{
    public class Friends : ApiBase
    {
        public Friends(ApiClient apiClient) : base(Enums.Version.v1, apiClient)
        {
        }

        public async Task AddFriend(int id, AccessToken personalInformation)
        {
            Requires(personalInformation, new[] { "friends" });
            await NoResponseRequest($"friends/{id}", personalInformation);
        }

        public async Task DeleteFriend(int id, AccessToken personalInformation)
        {
            Requires(personalInformation, new[] { "friends" });
            await NoResponseRequest($"friends/{id}", personalInformation, method: "DELETE");
        }
    }
}