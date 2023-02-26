using System.Threading.Tasks;

using BotShared.ShikimoriSharp;
using BotShared.ShikimoriSharp.AdditionalRequests;
using BotShared.ShikimoriSharp.Bases;

namespace BotShared.ShikimoriSharp.Information
{
    public class Forums : ApiBase
    {
        public Forums(ApiClient apiClient) : base(Enums.Version.v1, apiClient)
        {
        }

        public async Task<Forum[]> GetForums(AccessToken personalInformation = null)
        {
            return await Request<Forum[]>("forums", personalInformation);
        }
    }
}