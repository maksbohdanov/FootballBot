using System;
using System.Collections.Generic;
using Telegram.Bot;
using Telegram.Bot.Args;
using TgBot.Commands;
using TgBot.Models;

namespace TgBot
{
    public class Program
    {
        public static TelegramBotClient client = AppSettings.Configure(ref client);

        public static List<FixtureResponse> mathes = new List<FixtureResponse>();

        static void Main(string[] args)
        {
            AppSettings.Start();
        }
       
    }
}
