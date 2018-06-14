using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace gdbcLeaderBoard.Models.HomeViewModels
{
    public class ScoreOverviewViewModel
    {
        public List<VenueScoreViewModel> VenueScores { get; set; }
        public List<TeamScoreViewModel> TeamScores { get; set; }

        public string Title { get; set; }
        public bool SortOnAverage { get; set; }
    }
}
