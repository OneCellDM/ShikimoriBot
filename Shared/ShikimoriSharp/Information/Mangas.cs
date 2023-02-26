using System.Threading.Tasks;

using BotShared.ShikimoriSharp;
using BotShared.ShikimoriSharp.Bases;

namespace BotShared.ShikimoriSharp.Information
{
    public class Mangas : MangaRanobeApiBase
    {
        public Mangas(ApiClient apiClient) : base("mangas", apiClient)
        {
        }
    }
}