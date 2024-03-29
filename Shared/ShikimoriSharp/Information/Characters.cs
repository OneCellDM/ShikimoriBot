﻿using System.Threading.Tasks;

using BotShared.ShikimoriSharp;
using BotShared.ShikimoriSharp.Bases;
using BotShared.ShikimoriSharp.Classes;

namespace BotShared.ShikimoriSharp.Information
{
    public class Characters : ApiBase
    {
        public Characters(ApiClient apiClient) : base(Enums.Version.v1, apiClient)
        {
        }

        public async Task<Character[]> GetCharactersBySearch(string search)
        {
            return await Request<Character[], Search>("characters/search", new Search { search = search });
        }

        public async Task<FullCharacter> GetCharacterById(long id, AccessToken personalInformation = null)
        {
            return await Request<FullCharacter>($"characters/{id}", personalInformation);
        }
    }
}