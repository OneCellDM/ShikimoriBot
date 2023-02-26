using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotShared
{
    /// <summary>
    /// Класс для обработки комманд бота
    /// </summary>
    public class BotMessageHandler
    {
        private IBot Bot { get; set; }
        bool pollingEvent = false;
        public BotMessageHandler(IBot bot)
        {
            Bot = bot;

        }


        private async Task MessageLongPollEventHandler(long? chatId, string message)
        {
            var cmdArgs = SplitMessageArgs(message);


            if (cmdArgs == null || cmdArgs.Length == 0
                || chatId == null
                || message == null || message.Length == 0)
            {
                return;
            }
            long _chatId = (long)chatId;
            try
            {

                switch (cmdArgs[0])
                {

                    case "/randomanime":
                        {
                            await Bot.SendRandomAnimeAsync(_chatId);
                            break;
                        }

                    case "/randommanga":
                        {
                            await Bot.SendRandomMangaAsync(_chatId);
                            break;
                        }

                    case "/manga":
                        {
                            bool ok = false;
                            if (cmdArgs.Length > 1)
                            {

                                if (long.TryParse(cmdArgs[1], out long id))
                                {
                                    await Bot.SendMangaDetailAsync(_chatId, id);
                                }

                            }

                            break;
                        }

                    case "/anime":
                        {
                            bool ok = false;
                            if (cmdArgs.Length > 1)
                            {

                                if (long.TryParse(cmdArgs[1], out long id))
                                {
                                    await Bot.SendAnimeDetailAsync(_chatId, id);
                                }

                            }


                            break;
                        }
                  

                }
                await Bot.SendDefaultMessageAsync(_chatId);


            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка:" + ex.Message);
                Console.WriteLine(ex);

            }
        }

        public string[]? SplitMessageArgs(string? text) => text?.Trim().Split(" ").Where(x => x.Length > 0).ToArray();


        public async Task<bool> Start()
        {

            if (await Bot.Start())
            {
                if (pollingEvent == false)
                {
                    Bot.MessageLongPollEvent += MessageLongPollEventHandler;
                    pollingEvent = true;
                }
                return true;

            }
            return false;
        }
        public async Task<bool> Stop()
        {
            if (pollingEvent == true)
            {
                Bot.MessageLongPollEvent -= MessageLongPollEventHandler;
            }

            return await Bot.Stop();
        }
        public async Task Init() => await Bot.Init();

    }
}
