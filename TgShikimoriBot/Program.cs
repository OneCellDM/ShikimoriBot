using System.Xml.Linq;
using System;
using Microsoft.VisualBasic;
using static TgShikimoriBot.Program;
using Newtonsoft.Json.Linq;
using BotShared;

namespace TgShikimoriBot
{

    public partial class Program
    {
        

        static BotMessageHandler BotMessageHandler { get; set; }
     

        public static async Task Main(string[] args)
        {

            BotMessageHandler = new BotMessageHandler(new BotClient());

            var startRes = false;

            while (startRes == false)
            {
                await BotMessageHandler.Init();
                startRes = await BotMessageHandler.Start();
                
            }
          
            
            while (true)
            {
                Console.WriteLine("/stop остановить бота");
                if (Console.ReadLine() == "/stop")
                {
                    await BotMessageHandler.Stop();
                    break;
                    
                }
            }

          
        }

    }
}



