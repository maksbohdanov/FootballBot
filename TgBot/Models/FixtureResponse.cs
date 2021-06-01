using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TgBot.Models
{
    public class FixtureResponse
    {
        public string Id { get; set; }
        public string IdChat { get; set; }
        public int? IdFixture { get; set; }
        public string Date { get; set; }
        public string Stadium { get; set; }
        public string City { get; set; }
        public string Status { get; set; }
        public OddsResponse Odds { get; set; }
        public LeagueFixtureResponse League { get; set; }
        public TeamFixtureResponse Teams { get; set; }

    }
    public class LeagueFixtureResponse
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public int? Season { get; set; }

    }
    public class TeamFixtureResponse
    {
        public int IdHome { get; set; }
        public string NameHome { get; set; }
        public int? GoalHome { get; set; }


        public int IdAway { get; set; }
        public string NameAway { get; set; }
        public int? GoalAway { get; set; }

    }
}
