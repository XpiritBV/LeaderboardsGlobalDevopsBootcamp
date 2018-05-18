using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace gdbcLeaderBoard.Models
{
    public class Challenge
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int Points { get; set; }
        public string HelpUrl { get; set; }
        public bool IsBonus { get; set; }
    }

    public class TeamScoreItem
    {
        public int Id { get; set; }
        [Required]
        public int ChallengeID { get; set; }
        public Challenge Challenge { get; set; }
        [Required]
        public int TeamID { get; set; }
        public Team Team { get; set; }
        public bool HelpUsed { get; set; }
        public string Status { get; set; }
    }


    public class Team
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int VenueID { get; set; }
        public Venue Venue { get; set; }
        public ICollection<TeamScoreItem> Scores { get; set; }


    }

    public class Venue
    {
        public int Id { get; set; }

        public string VenueAdminID { get; set; }
        public ApplicationUser VenueAdmin { get; set; }
        [Required]
        public string Name { get; set; }
        public ICollection<Team> Teams { get; set; }

    }
}
