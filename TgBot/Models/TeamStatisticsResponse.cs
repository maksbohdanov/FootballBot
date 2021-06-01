using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TgBot.Models
{
    public class TeamStatisticsResponse
    {
        public int Id { get; set; }
        public string Team { get; set; }
        public int LeagueId { get; set; }
        public string League { get; set; }
        public int Season { get; set; }
        public GamesTeamStatResp Games { get; set; }
        public GoalsTeamStatResp Goals { get; set; }
        public PenaltyTeamStatResp Penalties { get; set; }
       
    }
    public class GamesTeamStatResp
    {
        public int Total { get; set; }
        public int Wins { get; set; }
        public int Loses { get; set; }
        public int Draws { get; set; }
        public int? CleanSheet { get; set; }
    }
    public class GoalsTeamStatResp
    {
        public int TotalFor { get; set; }
        public string AverageFor { get; set; }
        public int TotalAgainst { get; set; }
        public string AverageAgainst { get; set; }

    }
    public class PenaltyTeamStatResp
    {
        public int? Total { get; set; }
        public int? Scored { get; set; }
        public int? Missed { get; set; }

    }
   

}
