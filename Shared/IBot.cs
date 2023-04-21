using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotShared
{
    public interface IBot
    {
        public delegate Task LongPoll(long? chatId, string message);
        public event LongPoll MessageLongPollEvent;
        public Task Init();
        public Task<bool> Start();
        public Task<bool> Stop();
        public Task SendRandomAnimeAsync(long chatId);
        public Task SendRandomMangaAsync(long chatId);
        public Task SendAnimeDetailAsync(long chatId, long animeId);
        public Task SendMangaDetailAsync(long chatId, long mangaId);
        public Task SendDefaultMessageAsync(long chatId);
        public Task SendHelpMessageAsync(long chatId);
    }
    public interface IBot<T> : IBot
        where T : class
    {
        public T BotApi { get; set; }

    }
}
