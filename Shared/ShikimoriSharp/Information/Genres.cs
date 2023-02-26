using System.Threading.Tasks;

using BotShared.ShikimoriSharp;
using BotShared.ShikimoriSharp.Bases;
using BotShared.ShikimoriSharp.Classes;

namespace BotShared.ShikimoriSharp.Information
{
    public class Genres : ApiBase
    {
        public Genres(ApiClient apiClient) : base(Enums.Version.v1, apiClient)
        {
        }

        public async Task<Genre[]> GetGenres()
        {
            return await Request<Genre[]>("genres");
        }
    }
}