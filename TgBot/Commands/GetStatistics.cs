using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TgBot.Models;

namespace TgBot.Commands
{
    public static class GetStatistics
    {
        private static string team;
        private static string league;
        public static async Task Execute(MessageEventArgs message)
        {
            await Program.client.SendTextMessageAsync(message.Message.From.Id, @"Enter the <b><i>full</i></b> team name (<i>Real Madrid, Chelsea, etc...</i>)", ParseMode.Html);

            Program.client.OnMessage += GetQueryParams;
            Bot.IsSubscribed = true;


        }

        private static async void GetQueryParams(object sender, MessageEventArgs e)
        {
            team = e.Message.Text;

            await Program.client.SendTextMessageAsync(e.Message.From.Id, @"Enter the <b><i>full</i></b> league name (<i>UEFA Champions League, Primera Division, Premier League, etc...</i>)", ParseMode.Html);


            Program.client.OnMessage -= GetQueryParams;
            Program.client.OnMessage += GetQueryParams2;

        }

        private static async void GetQueryParams2(object sender, MessageEventArgs e)
        {
            league = e.Message.Text;

            var stat = await ApiRepository.GetStatistics($"{ApiClient.Base}/TeamStatistics/statName?team={team}&league={league}");

            if (stat == null)
            {
                await Program.client.SendTextMessageAsync(e.Message.From.Id, @$"<b>Something went wrong!</b>
Check if you entered team '<i>{team}</i>' and league '<i>{league}</i>' name correctly and send the last command again", parseMode: ParseMode.Html);
                Program.client.OnMessage -= GetQueryParams2;
                Bot.IsSubscribed = false;

                return;
            }

            string text = $@"<b>{stat.Team}</b>

<b><i>League:</i></b>   {league}
<b><i>Season:</i></b>   {stat.Season}

<b><i>Wins:</i></b>   {stat.Games.Wins}
<b><i>Draws:</i></b>   {stat.Games.Draws}
<b><i>Loses:</i></b>   {stat.Games.Loses}
<b><i>Total:</i></b>   {stat.Games.Total}

<b><i>Scored:</i></b>   {stat.Goals.TotalFor} ({stat.Goals.AverageFor})
<b><i>Conceded:</i></b>   {stat.Goals.TotalAgainst} ({stat.Goals.AverageAgainst})
<b><i>CleanSheets:</i></b>   {stat.Games?.CleanSheet ?? 0 }

<b><i>Penalties:</i></b>   {stat.Penalties?.Total ?? 0 }
<b><i>Scored:</i></b>   {stat.Penalties?.Scored ?? 0 }
<b><i>Conceded:</i></b>   {stat.Penalties?.Missed ?? 0 }";

            await Program.client.SendTextMessageAsync(e.Message.From.Id, text, parseMode: ParseMode.Html);


            Program.client.OnMessage -= GetQueryParams2;
            Bot.IsSubscribed = false;


        }
    }
}
