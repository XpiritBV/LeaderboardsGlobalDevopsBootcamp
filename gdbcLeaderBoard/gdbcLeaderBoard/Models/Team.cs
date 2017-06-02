using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace gdbcLeaderBoard.Models
{
    public class Challenge
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Points { get; set; }
    }

    public class TeamScoreItem {
        public int Id { get; set; }
        public Challenge Challenge { get; set; }
        public Team Team { get; set; }
    }


    public class Team
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Venue Venue { get; set; }
        public ICollection<TeamScoreItem> Scores { get; set; }
    }

    public class Venue
    {
        public int Id { get; set; }
        public ApplicationUser VenueAdmin { get;set; }
        public string Name { get; set; }
        public ICollection<Team> Teams { get; set; }
    }
}
