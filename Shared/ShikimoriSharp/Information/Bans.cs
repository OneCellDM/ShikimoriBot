using System.Threading.Tasks;

using BotShared.ShikimoriSharp;
using BotShared.ShikimoriSharp.Bases;
using BotShared.ShikimoriSharp.Classes;

namespace BotShared.ShikimoriSharp.Information
{
    public class Bans : ApiBase
    {
        public Bans(ApiClient apiClient) : base(Enums.Version.v1, apiClient)
        {
        }

        public async Task<Ban[]> GetBans()
        {
            return await Request<Ban[]>("bans");
        }
    }
}