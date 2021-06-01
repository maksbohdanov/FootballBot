using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using TgBot.Models;

namespace TgBot.Commands
{
    public static class GetUpcoming
    {
        public static InlineKeyboardMarkup inlineKeyboard { get; set; }
        
        public static List<FixtureResponse> upcoming = new List<FixtureResponse>();

        public static async Task Execute(MessageEventArgs message)
        {
            Program.client.OnCallbackQuery -= GetUpcoming.ButtonPressed;
            Program.client.OnCallbackQuery -= GetLast.ButtonPressed;


            await Program.client.SendTextMessageAsync(message.Message.From.Id, @"Enter the <b><i>full</i></b> team name (<i>Real Madrid, Chelsea, etc...</i>)", ParseMode.Html);

            Program.client.OnMessage += GetQueryParams;
            Bot.IsSubscribed = true;


        }
        private static async void GetQueryParams(object sender, MessageEventArgs e)
        {
            var team = e.Message.Text;

            var fixtures = await ApiRepository.GetFixtures($"{ApiClient.Base}/Fixture/teamUpcoming?team={team}");

            if (fixtures == null)
            {
                await Program.client.SendTextMessageAsync(e.Message.From.Id, @$"<b>Ooops!</b>
I couldn't find upcoming matches for the team '<i>{team}</i>'. Maybe you entered the team name incorrectly or there is no information about upcoming matches.", parseMode: ParseMode.Html);
                Program.client.OnMessage -= GetQueryParams;
                Bot.IsSubscribed = false;
                return;
            }



            foreach (var fixture in fixtures)
            {
                List<InlineKeyboardButton[]> ToAddList = new List<InlineKeyboardButton[]>();

                upcoming.Add(fixture);
                string home = fixture.Odds.Home != null ? fixture.Odds.Home : "no data";
                string draw = fixture.Odds.Draw != null ? fixture.Odds.Draw : "no data";
                string away = fixture.Odds.Away != null ? fixture.Odds.Away : "no data";
                string data = fixture.Date != null ? fixture.Date : "no data";
                string country = fixture.League.Country != null ? fixture.League.Country : "no data";
                string stadium = fixture.Stadium != null ? fixture.Stadium : "no data";
                string city = fixture.City != null ? fixture.City : "no data";


                string text =
                        $@"<b>{fixture.Teams.NameHome}      {fixture.Teams.GoalHome} : {fixture.Teams.GoalAway}       {fixture.Teams.NameAway}</b>

<b><i>Home:</i></b>     {home}
<b><i>Draw:</i></b>     {draw}
<b><i>Away:</i></b>     {away}

<b><i>Data:</i></b>  {data}
<b><i>League:</i></b>  {fixture.League.Name}
<b><i>Season:</i></b>  {fixture.League.Season}
<b><i>Country:</i></b>  {country}
<b><i>Stadium:</i></b>  {stadium}
<b><i>City:</i></b>  {city}
";

                var dataStat = fixture.Teams.NameHome + "," + fixture.Teams.NameAway + "," + fixture.League.Name;
                var dataStand = fixture.League.Name + "," + fixture.League.Country;


                InlineKeyboardButton button = new InlineKeyboardButton() { CallbackData ="A" + (upcoming.Count -1).ToString(), Text = "⭐️" };
                InlineKeyboardButton buttonStat = new InlineKeyboardButton() { CallbackData = "B" + dataStat, Text = "Statistics" };
                InlineKeyboardButton buttonStandinds = new InlineKeyboardButton() { CallbackData = "C" + dataStand, Text = "Standinds" };

                InlineKeyboardButton[] row1 = new InlineKeyboardButton[1] { button };
                InlineKeyboardButton[] row2 = new InlineKeyboardButton[2] { buttonStat, buttonStandinds };

                ToAddList.Add(row1);
                ToAddList.Add(row2);

                inlineKeyboard = new InlineKeyboardMarkup(ToAddList);

                await Program.client.SendTextMessageAsync(e.Message.From.Id, text, parseMode: ParseMode.Html, replyMarkup: inlineKeyboard);

            }

            Program.client.OnCallbackQuery += ButtonPressed;
            Program.client.OnMessage -= GetQueryParams;
            Bot.IsSubscribed = false;

        }

        public static async void ButtonPressed(object sender, CallbackQueryEventArgs e)
        {
            try
            {
                
                string buttonText;

                if (e.CallbackQuery.Data.StartsWith("A"))
                {
                    var additional = inlineKeyboard.InlineKeyboard.ToList();
                    if (additional.Count > 1)
                    {
                        additional.Remove(additional[0]);
                    }
                   
                    InlineKeyboardMarkup KeyboardAdditional = new InlineKeyboardMarkup(additional);


                    await Program.client.EditMessageReplyMarkupAsync(e.CallbackQuery.Message.Chat.Id, e.CallbackQuery.Message.MessageId, KeyboardAdditional);

                    buttonText = e.CallbackQuery.Data.Substring(1);

                    var toAdd = upcoming[Int32.Parse(buttonText)];

                    await ApiRepository.AddToFavor($"{ApiClient.Base}/Fixture/add", e.CallbackQuery.Message.Chat.Id.ToString(), toAdd);

                    return;
                }
                else if (e.CallbackQuery.Data.StartsWith("B"))
                {
                    buttonText = e.CallbackQuery.Data.Substring(1);
                    var parameters = buttonText.Split(",");
                    var team1 = parameters[0];
                    var team2 = parameters[1];
                    var league = parameters[2];

                    var stat1 = await ApiRepository.GetStatistics($"{ApiClient.Base}/TeamStatistics/statName?team={team1}&league={league}");
                    var stat2 = await ApiRepository.GetStatistics($"{ApiClient.Base}/TeamStatistics/statName?team={team2}&league={league}");


                    var text1 = $@"<b>{team1}</b>

<b><i>League:</i></b>   {league}
<b><i>Season:</i></b>   {stat1.Season}

<b><i>Wins:</i></b>   {stat1.Games.Wins}
<b><i>Draws:</i></b>   {stat1.Games.Draws}
<b><i>Loses:</i></b>   {stat1.Games.Loses}
<b><i>Total:</i></b>   {stat1.Games.Total}

<b><i>Scored:</i></b>   {stat1.Goals.TotalFor} ({stat1.Goals.AverageFor})
<b><i>Conceded:</i></b>   {stat1.Goals.TotalAgainst} ({stat1.Goals.AverageAgainst})
<b><i>CleanSheets:</i></b>   {stat1.Games?.CleanSheet ?? 0 }

<b><i>Penalties:</i></b>   {stat1.Penalties?.Total ?? 0 }
<b><i>Scored:</i></b>   {stat1.Penalties?.Scored ?? 0 }
<b><i>Conceded:</i></b>   {stat1.Penalties?.Missed ?? 0 }";
                    var text2 = $@"<b>{team2}</b>

<b><i>League:</i></b>   {league}
<b><i>Season:</i></b>   {stat2.Season}

<b><i>Wins:</i></b>   {stat2.Games.Wins}
<b><i>Draws:</i></b>   {stat2.Games.Draws}
<b><i>Loses:</i></b>   {stat2.Games.Loses}
<b><i>Total:</i></b>   {stat2.Games.Total}

<b><i>Scored:</i></b>   {stat2.Goals.TotalFor} ({stat2.Goals.AverageFor})
<b><i>Conceded:</i></b>   {stat2.Goals.TotalAgainst} ({stat2.Goals.AverageAgainst})
<b><i>CleanSheets:</i></b>   {stat2.Games?.CleanSheet ?? 0 }

<b><i>Penalties:</i></b>   {stat2.Penalties?.Total ?? 0 }
<b><i>Scored:</i></b>   {stat2.Penalties?.Scored ?? 0 }
<b><i>Conceded:</i></b>   {stat2.Penalties?.Missed ?? 0 }";

                    await Program.client.SendTextMessageAsync(e.CallbackQuery.Message.Chat.Id, text1, ParseMode.Html);
                    await Program.client.SendTextMessageAsync(e.CallbackQuery.Message.Chat.Id, text2, ParseMode.Html);

                }
                else if (e.CallbackQuery.Data.StartsWith("C"))
                {
                    buttonText = e.CallbackQuery.Data.Substring(1);
                    var parameters = buttonText.Split(",");
                    var league = parameters[0];
                    var country = parameters[1];

                    var standings = await ApiRepository.GetStandings($"{ApiClient.Base}/TeamStatistics/standings?league={league}&country={country}");
                    if (standings == null)
                    {
                        await Program.client.SendTextMessageAsync(e.CallbackQuery.Message.Chat.Id, @"<b>Ooops!</b>
Something went wrong.", ParseMode.Html);
                        return;

                    }

                    var text = @$"<b>{standings.League}     {standings.Season}</b>


";

                    foreach (var stand in standings.TeamsStat)
                    {
                        var teamResult =
   $@"<b><i>Rank:</i></b>    {stand.Rank}
Team:       {stand.Team}
Points:     {stand.Points}
Played:     {stand.Played}
Wins:    {stand.Win}
Draws:    {stand.Draw}
Loses:    {stand.Lose}

Scored:    {stand.GoalsFor}
Conceded:    {stand.GoalsAgainst}
Difference:   {stand.GoalsDiff}



";

                        text += teamResult;
                    }
                    await Program.client.SendTextMessageAsync(e.CallbackQuery.Message.Chat.Id, text, ParseMode.Html);

                }

            }
            catch (Exception)
            {
                return;
                throw;
            }
            
        }
    }
}

