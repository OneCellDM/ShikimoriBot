using Microsoft.Extensions.Logging;
using ShikimoriSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TgShikimoriBot
{
    public static class ShikimoriApiHandler
    {
        class Logger : Microsoft.Extensions.Logging.ILogger
        {
            public IDisposable BeginScope<TState>(TState state)
            {
                throw new NotImplementedException();
            }

            public bool IsEnabled(Microsoft.Extensions.Logging.LogLevel logLevel)
            {
                return true;
            }

           

            public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
            {
                Debug.WriteLine(state);
            }
        }




        private static bool _IsInit = false;
        private static ShikimoriClient? _ApiClient;
    
        public static ShikimoriClient? ApiClient { get; set; }

        static ShikimoriApiHandler()
        {
            ApiClient = new ShikimoriClient(

                   new Logger(),
                   new("ShikikomoriOneCellDMApp", "To5K_6HfyRpSp_ZpOap8WReJFkjv7O3_Wj7dKl4_X90", "s-gUpfM5icFV18PI0ljmM0rpxBPiMqhNjC113C_0hFo")

                );


        }
    }
}
