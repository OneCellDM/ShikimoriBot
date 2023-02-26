using System.Threading.Tasks;

using BotShared.ShikimoriSharp;
using BotShared.ShikimoriSharp.Bases;
using BotShared.ShikimoriSharp.Classes;
using BotShared.ShikimoriSharp.Settings;

namespace BotShared.ShikimoriSharp.UpdatableInformation
{
    public class Styles : ApiBase
    {
        public Styles(ApiClient apiClient) : base(Enums.Version.v1, apiClient)
        {
        }

        public async Task<Style> GetStyle(int id)
        {
            return await Request<Style>($"styles/{id}");
        }

        public async Task<Style> PreviewStyle(StylePreviewSettings settings, AccessToken personalInformation = null)
        {
            return await SendJson<Style>("styles/preview", settings.style, personalInformation);
        }

        public async Task<Style> CreateStyle(StyleSettings settings, AccessToken personalInformation)
        {
            return await SendJson<Style>("styles/preview", settings.style, personalInformation);
        }

        public async Task<Style> UpdateStyle(int id, StyleUpdateSettings settings, AccessToken personalInformation)
        {
            return await SendJson<Style>($"styles/{id}", settings.style, personalInformation);
        }
    }
}