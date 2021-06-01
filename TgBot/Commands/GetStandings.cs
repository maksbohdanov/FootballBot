using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;

namespace TgBot.Commands
{
    public static class GetStandings
    {
        private static string league;
        private static string country;
        public static async Task Execute(MessageEventArgs message)
        {
            await Program.client.SendTextMessageAsync(message.Message.From.Id, @"Enter the <b><i>full</i></b> league name (<i>Premier League, Primera Division, etc...</i>)
", ParseMode.Html);

            Program.client.OnMessage += GetQueryParams;
            Bot.IsSubscribed = true;
        }
        private static async void GetQueryParams(object sender, MessageEventArgs e)
        {
            league = e.Message.Text;

            await Program.client.SendTextMessageAsync(e.Message.From.Id, @"Enter the country name (<i>Ukraine, England, Spain, etc...</i>)", ParseMode.Html);


            Program.client.OnMessage -= GetQueryParams;
            Program.client.OnMessage += GetQueryParams2;
        }

        private static async void GetQueryParams2(object sender, MessageEventArgs e)
        {
            country = e.Message.Text;

            var standings = await ApiRepository.GetStandings($"{ApiClient.Base}/TeamStatistics/standings?league={league}&country={country}");

            if (standings == null)
            {
                await Program.client.SendTextMessageAsync(e.Message.From.Id, @$"<b>Something went wrong!</b>
Check if you entered league '<i>{league}</i>' and country '<i>{country}</i>' name correctly and send the last command again.", parseMode: ParseMode.Html);
                Program.client.OnMessage -= GetQueryParams2;
                Bot.IsSubscribed = false;

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
            await Program.client.SendTextMessageAsync(e.Message.From.Id, text, parseMode: ParseMode.Html);


            Program.client.OnMessage -= GetQueryParams2;
            Bot.IsSubscribed = false;


        }

    }
}
