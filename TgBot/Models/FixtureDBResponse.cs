using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TgBot.Models
{
    public class FixtureDBResponse
    {
        public string Id { get; set; }
        public string IdChat { get; set; }

        public int? IdFixture { get; set; }
        public string Date { get; set; }
        public string Stadium { get; set; }
        public string City { get; set; }
        public string Status { get; set; }
        public int? IdLeague { get; set; }
        public string League { get; set; }
        public string Country { get; set; }
        public int? Season { get; set; }

        public string Home { get; set; }
        public string Draw { get; set; }
        public string Away { get; set; }

        public int IdHome { get; set; }
        public string NameHome { get; set; }
        public string GoalHome { get; set; }


        public int IdAway { get; set; }
        public string NameAway { get; set; }
        public string GoalAway { get; set; }
    }
}
