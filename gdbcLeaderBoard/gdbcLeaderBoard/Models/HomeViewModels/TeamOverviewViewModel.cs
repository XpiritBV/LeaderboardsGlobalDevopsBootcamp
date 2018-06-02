using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace gdbcLeaderBoard.Models.HomeViewModels
{
    public class TeamOverviewViewModel
    {
        public List<Venue> Venues { get; set; }

        [Display(Name = "Venues")]
        public int Id { get; set; }

        public string Name { get; set; }

        public IEnumerable<SelectListItem> VenueItems
        {
            get { return new SelectList(Venues, "Name", "Name"); }
        }

        public List<TeamScoreViewModel> TeamScores { get; set; }
    }
}
