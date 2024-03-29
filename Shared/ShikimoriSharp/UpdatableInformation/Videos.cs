﻿using System.Threading.Tasks;

using BotShared.ShikimoriSharp;
using BotShared.ShikimoriSharp.Bases;
using BotShared.ShikimoriSharp.Classes;

namespace BotShared.ShikimoriSharp.UpdatableInformation
{
    public class Videos : ApiBase
    {
        public Videos(ApiClient apiClient) : base(Enums.Version.v1, apiClient)
        {
        }

        public async Task<Video> GetVideos(int id)
        {
            return await Request<Video>($"animes/{id}/videos");
        }

        public async Task<Video> PostVideo(int id, NewVideo video, AccessToken personalInformation)
        {
            return await SendJson<Video>($"animes/{id}/videos", video, personalInformation);
        }

        public async Task DeleteVideo(int a_id, int id, AccessToken personalInformation)
        {
            await NoResponseRequest($"animes/{a_id}/videos/{id}", personalInformation);
        }
    }
}