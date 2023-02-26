using System.Threading.Tasks;

using BotShared.ShikimoriSharp.Bases;


namespace BotShared.ShikimoriSharp.Information
{
    public class Ranobe : MangaRanobeApiBase
    {
        public Ranobe(ApiClient apiClient) : base("ranobe", apiClient)
        {
        }
    }
}