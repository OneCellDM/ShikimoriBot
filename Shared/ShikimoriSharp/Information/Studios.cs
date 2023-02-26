using System.Threading.Tasks;

using BotShared.ShikimoriSharp.Bases;
using BotShared.ShikimoriSharp.Classes;

namespace BotShared.ShikimoriSharp.Information
{
    public class Studios : ApiBase
    {
        public Studios(ApiClient apiClient) : base(Enums.Version.v1, apiClient)
        {
        }

        public async Task<Studio[]> GetStudios()
        {
            return await Request<Studio[]>("studios");
        }
    }
}