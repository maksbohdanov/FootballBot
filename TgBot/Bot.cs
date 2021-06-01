using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;
using TgBot.Commands;
using TgBot.Models;

namespace TgBot
{
    public static class Bot
    {
        public static bool IsSubscribed=false;
        public static async void OnMessage(object sender, MessageEventArgs e)
        {
            if (e.Message.Text != null)
            {
                var chatID = e.Message.Chat.Id;
                var message = e.Message;
                var messageText = e.Message.Text;

                Console.WriteLine($"Bot received a message '{messageText}'  in chat {chatID}.");
                if (!IsSubscribed)
                {
                    switch (messageText)
                    {
                        case "/start":
                            string startText =
                                    $@"Hello! I can help you find out information about football matches and more
To learn how to use bot send /help
";
                            await Program.client.SendTextMessageAsync(message.From.Id, startText, parseMode: ParseMode.Html);
                            break;
                        case "/help":
                            string helpText = @"Use these commands:
/last - get last matches of the team
/upcoming - get upcoming matches of the team
/statistics - get team statistics in some tournament, league
/standings - get standings for a league 
/favorites - get your favorite matches
/delete - delete match from favorites";

                            await Program.client.SendTextMessageAsync(message.From.Id, helpText, parseMode: ParseMode.Html);
                            break;

                        case "/favorites":
                            await GetFavorites(e);
                            break;
                        case "/last":
                            await GetLast.Execute(e);
                            break;
                        case "/upcoming":
                            await GetUpcoming.Execute(e);
                            break;
                        case "/statistics":
                            await GetStatistics.Execute(e);
                            break;
                        case "/standings":
                            await GetStandings.Execute(e);
                            break;
                        case "/delete":
                            await Delete.Execute(e);
                            break;
                        default:
                            await ExtensionCommand(e);
                            break;
                    }
                }
                
            }
        }
        
        public static async Task ExtensionCommand(MessageEventArgs e)
        {
            
                if (!IsSubscribed)
                {
                    await Program.client.SendTextMessageAsync(e.Message.From.Id, @"<b>Unknown command</b>
See /help for available commands.", parseMode: ParseMode.Html);
                }
                
            
        }
        public static async Task GetFavorites( MessageEventArgs eve)
        {          
            List<FixtureResponse> fixtures = await ApiRepository.GetAllFavorite($"{ApiClient.Base}/Fixture/all_favorites?user={eve.Message.Chat.Id}");
            if (fixtures == null)
            {
                await Program.client.SendTextMessageAsync(eve.Message.From.Id, @"You don't have any favorite matches yet.", parseMode: ParseMode.Html);
                return;
            }


            foreach (var fixture in fixtures)
            {
                string text =
                        $@"<b>{fixture.Teams.NameHome}      {fixture.Teams.GoalHome} : {fixture.Teams.GoalAway}       {fixture.Teams.NameAway}</b>

<b><i>Home:</i></b>     {fixture.Odds.Home}
<b><i>Draw:</i></b>     {fixture.Odds.Draw}
<b><i>Away:</i></b>     {fixture.Odds.Away}

<b><i>Data:</i></b>  {fixture.Date}
<b><i>League:</i></b>  {fixture.League.Name}
<b><i>Season:</i></b>  {fixture.League.Season}
<b><i>Country:</i></b>  {fixture.League.Country}
<b><i>Stadium:</i></b>  {fixture.Stadium}
<b><i>City:</i></b>  {fixture.City}
";


                await Program.client.SendTextMessageAsync(eve.Message.From.Id, text, parseMode: ParseMode.Html);
            }
        }

    }
}