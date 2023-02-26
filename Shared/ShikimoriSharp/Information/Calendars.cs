using System.Threading.Tasks;

using BotShared.ShikimoriSharp;
using BotShared.ShikimoriSharp.Bases;
using BotShared.ShikimoriSharp.Classes;

namespace BotShared.ShikimoriSharp.Information
{
    public class Calendars : ApiBase
    {
        public Calendars(ApiClient apiClient) : base(Enums.Version.v1, apiClient)
        {
        }

        public async Task<Calendar[]> GetCalendar()
        {
            return await Request<Calendar[]>("calendar");
        }
    }
}