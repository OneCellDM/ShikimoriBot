using System.Threading.Tasks;

using BotShared.ShikimoriSharp.Bases;
using BotShared.ShikimoriSharp.Classes;
using BotShared.ShikimoriSharp.Settings;

namespace BotShared.ShikimoriSharp.UpdatableInformation
{
    public class UserImages : ApiBase
    {
        public UserImages(ApiClient apiClient) : base(Enums.Version.v1, apiClient)
        {
        }

        public async Task<ResultImage> CreateUserImage(UserImagesSettings settings, AccessToken personalInformation)
        {
            Requires(personalInformation, new[] { "comments" });
            return await Request<ResultImage, UserImagesSettings>("user_images", settings, personalInformation, "POST");
        }
    }
}