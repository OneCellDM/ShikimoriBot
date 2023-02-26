using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotShared
{
    public struct BotCommand
    {
        public string Title { get; init; }
        public string Command { get; init; }
    }
    public static class BotConstans
    {
        public static readonly string SendScreenShots = "Отправка скриншотов";
        public static readonly string InputAction = "Нажмите на кнопку или введите команду";

        public static readonly BotCommand RandomAnime = new BotCommand()
        {
            Title = "Случайное аниме",
            Command = "/randomanime",

        };
        public static readonly BotCommand RandomManga = new BotCommand()
        {
            Title = "Случайная манга",
            Command = "/randommanga",

        };

        public static readonly BotCommand GetManga = new BotCommand()
        {
            Title = "Подробнее",
            Command = "/manga"
        };
        public static readonly BotCommand GetAnime = new BotCommand()
        {
            Title = "Подробнее",
            Command = "/anime"
        };

    }
}
