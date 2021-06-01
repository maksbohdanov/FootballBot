using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TgBot.Models
{
    public class StandingsResponse
    {
        public string League { get; set; }
        public string Country { get; set; }
        public int Season { get; set; }
        public List<Stat>  TeamsStat { get; set; }
}
    public class Stat
    {
        public int Id { get; set; }
        public string Team { get; set; }
        public int Rank { get; set; }
        public int Points { get; set; }
        public int Played { get; set; }
        public int Win { get; set; }
        public int Draw { get; set; }
        public int Lose { get; set; }
        public int GoalsFor { get; set; }
        public int GoalsAgainst { get; set; }
        public int GoalsDiff { get; set; }

    }
}
