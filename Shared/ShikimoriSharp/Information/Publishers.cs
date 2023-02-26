using System.Threading.Tasks;

using BotShared.ShikimoriSharp;
using BotShared.ShikimoriSharp.Bases;
using BotShared.ShikimoriSharp.Classes;

namespace BotShared.ShikimoriSharp.Information
{
    public class Publishers : ApiBase
    {
        public Publishers(ApiClient apiClient) : base(Enums.Version.v1, apiClient)
        {
        }

        public async Task<Publisher[]> GetPublisher()
        {
            return await Request<Publisher[]>("publishers");
        }
    }
}