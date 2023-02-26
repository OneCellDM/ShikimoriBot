using System.Threading.Tasks;

using BotShared.ShikimoriSharp.Bases;
using BotShared.ShikimoriSharp.Classes;

namespace BotShared.ShikimoriSharp.Information
{
    public class People : ApiBase
    {
        public People(ApiClient apiClient) : base(Enums.Version.v1, apiClient)
        {
        }

        public async Task<SearchPerson[]> GetPerson(Search settings)
        {
            return await Request<SearchPerson[], Search>("people/search", settings);
        }

        public async Task<Person> GetPerson(long id, AccessToken personalInformation = null)
        {
            return await Request<Person>($"people/{id}", personalInformation);
        }
    }
}