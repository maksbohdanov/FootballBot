using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace TgBot
{
    class AppSettings
    {
        public static string Url { get; set; } = "https://localhost:44343/";
        public static string Name { get; set; } = "Football Bot";
        public static string Key { get; set; } = "xxxxxxxxxxxxx";


        public static TelegramBotClient Configure(ref TelegramBotClient client)
        {
            if (client != null)
            {
                return client;
            }

            client = new TelegramBotClient(AppSettings.Key);
            return client;
        }

        public static void Start()
        {
            Console.WriteLine("Bot has been started!" +
                "\n===========================================================");
            ApiClient.InitializeClient();
            Program.client.OnMessage += Bot.OnMessage;  
            
            Program.client.StartReceiving();
            Console.ReadLine();
            Program.client.StartReceiving();

        }
    }
}
