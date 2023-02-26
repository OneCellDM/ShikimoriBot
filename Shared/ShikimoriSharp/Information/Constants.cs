using System.Threading.Tasks;

using BotShared.ShikimoriSharp.Bases;
using BotShared.ShikimoriSharp.Classes.Constants;

namespace BotShared.ShikimoriSharp.Information
{
    public class Constants : ApiBase
    {
        public Constants(ApiClient apiClient) : base(Enums.Version.v1, apiClient)
        {
        }

        private async Task<ConstantsAnimeManga> Lesscode(string dest)
        {
            return await Request<ConstantsAnimeManga>(dest);
        }

        public async Task<ConstantsAnimeManga> GetAnimeConstants()
        {
            return await Lesscode("constants/anime");
        }

        public async Task<ConstantsAnimeManga> GetMangaConstants()
        {
            return await Lesscode("constants/manga");
        }

        public async Task<ConstantsUserRate> GetUserRateConstants()
        {
            return await Request<ConstantsUserRate>("constants/user_rate");
        }

        public async Task<ConstantsClub> GetClubConstants()
        {
            return await Request<ConstantsClub>("constants/club");
        }

        public async Task<ConstantsSmileys[]> GetSmileysConstants()
        {
            return await Request<ConstantsSmileys[]>("constants/smileys");
        }
    }
}