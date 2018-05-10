using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace gdbcLeaderBoard.Models.HomeViewModels
{
    public class TeamScoreViewModel
    {
        public string Venue { get; set; }
        public string Team { get; set; }
        public int Score { get; set; }
        public int InProgressCount { get; set; }
        public int DoneCount { get; set; }
    }
}
