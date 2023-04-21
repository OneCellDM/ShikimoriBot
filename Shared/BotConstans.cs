using BotShared.ShikimoriSharp.Classes;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BotShared
{
    public struct BotCommand
    {
        public string Title { get; init; }
        public string Command { get; init; }
        public string Example { get; init; }
    }
    public static class BotConstans
    {
        public static List<BotCommand> GetCommandsList()
        {
            List<BotCommand> commands = new List<BotCommand>();
            Type myType = typeof(BotConstans);
            foreach (FieldInfo field in myType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static))
            {
                string modificator = "";

                if (field.FieldType == typeof(BotCommand))
                {
                   BotCommand val = (BotCommand)field.GetValue(null);
                   commands.Add(val);
                   
                }  
            }
            return commands;
        }
        public static string GetCommandsString()
        {
            StringBuilder sb = new StringBuilder();
            var commands = GetCommandsList();
            foreach(var command in commands)
            {
                if (string.IsNullOrEmpty(command.Example)) 
                {
                    sb.AppendLine($"{command.Command} - {command.Title}");
                }
                else sb.AppendLine($"{command.Command} - {command.Title} \nПример: {command.Example}");
            }
            return sb.ToString();
        }

        public static readonly string SendScreenShots = "Отправка скриншотов";
        public static readonly string InputAction = "Нажмите на кнопку или введите команду";
        public static readonly BotCommand Help = new BotCommand()
        {
            Title = "Помощь",
            Command = "/help",
        };

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
            Title = "Подробнее о манге",
            Command = "/manga",
            Example="/manga id"
        };
        public static readonly BotCommand GetAnime = new BotCommand()
        {
            Title = "Подробнее о аниме",
            Command = "/anime",
            Example ="/anime id"
            
        };

    }
}
