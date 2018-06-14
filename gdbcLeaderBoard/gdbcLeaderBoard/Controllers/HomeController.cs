using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using gdbcLeaderBoard.Data;
using gdbcLeaderBoard.Models.HomeViewModels;
using Microsoft.EntityFrameworkCore;

namespace gdbcLeaderBoard.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(bool sortOnAverage)
        {
            ScoreOverviewViewModel vm = GetTopScores(sortOnAverage);
            int displayNumber = 50;
            vm.Title = $"Top {displayNumber} Venues";
            if (sortOnAverage)
            {
                vm.Title = $"{vm.Title} - Average per team";

            }
            vm.SortOnAverage = sortOnAverage;

            return View(vm);
        }

        private ScoreOverviewViewModel GetTopScores(bool sortOnAverage)
        {
            ScoreOverviewViewModel vm = new ScoreOverviewViewModel();


            var teamScores = _context.Team.Select(tt =>
                new TeamScoreViewModel { Venue = tt.Venue.Name, Team = tt.Name, Score = Convert.ToInt32(tt.Scores.Where(s => s.Status == "Done").Sum(s => (((decimal)s.Challenge.Points) * (s.HelpUsed ? 0.5m : 1m)))) }
            )
            .Where(t => t.Score > 0)
            .OrderByDescending(o => o.Score).Take(50)
            .ToList();


            vm.TeamScores = teamScores;

            List<VenueScoreViewModel> venueScores = _context.Team.Select(t =>
                new VenueScoreViewModel { Venue = t.Venue.Name, Score = Convert.ToInt32(t.Scores.Where(s => s.Status == "Done").Sum(s => ((s.Challenge.Points) * (s.HelpUsed ? 0.5m : 1m)))) }
            )
            .GroupBy(x => x.Venue)
            .Select(x =>
                new VenueScoreViewModel { Venue = x.Key, Score = x.Sum(s => s.Score), nrTeams = x.Count(), avgScore = (x.Sum(s => s.Score) / x.Count()) }
            )
            .Where(t => t.Score > 0)
            .ToList();

            if (sortOnAverage)
            {
                vm.Title = vm.Title + "(Average score per Team)";
                vm.VenueScores = venueScores.OrderByDescending(x => x.avgScore).ToList();
            }
            else
            {
                vm.VenueScores = venueScores.OrderByDescending(x => x.Score).ToList();

            }

            return vm;
        }

        public IActionResult IndexTeams()
        {
            ScoreOverviewViewModel vm = GetTopScores(false);
            int displayNumber = 50;

            vm.Title = $"Top {displayNumber} Teams";
            
            return View(vm);
        }

        public IActionResult Venues()
        {
            ScoreOverviewViewModel vm = new ScoreOverviewViewModel();

            var venueScores = _context.Team.Select(t =>
                new VenueScoreViewModel { Venue = t.Venue.Name, Score = Convert.ToInt32(t.Scores.Where(s => s.Status == "Done").Sum(s => ((s.Challenge.Points) * (s.HelpUsed ? 0.5m : 1m)))) }
            )
            .GroupBy(x => x.Venue)
            .Select(x =>
                new VenueScoreViewModel { Venue = x.Key, Score = x.Sum(s => s.Score) }
            )
            .Where(t => t.Score > 0)
            .OrderByDescending(o => o.Score)
            .ToList();

            vm.VenueScores = venueScores;

            return View(vm);
        }

        public IActionResult Teams(string id)
        {
            TeamOverviewViewModel vm = new TeamOverviewViewModel { Name = id };

            var teamScores = _context.Team.Select(tt =>
                new TeamScoreViewModel { Venue = tt.Venue.Name, Team = tt.Name, DoneCount = tt.Scores.Count(x => x.Status.ToLowerInvariant() == "done"), ApprovedCount = tt.Scores.Count(x => x.Status.ToLowerInvariant() == "approved"), CommitedCount = tt.Scores.Count(x => x.Status.ToLowerInvariant() == "commited"), Score =Convert.ToInt32( tt.Scores.Where(s => s.Status == "Done").Sum(s => ((s.Challenge.Points) * (s.HelpUsed ? 0.5m : 1m)))) }
            );

            if (id != null)
            {
                teamScores = teamScores.Where(v => v.Venue.ToLower() == id.ToLower() && v.Score > 0);
            }

            vm.TeamScores = teamScores.Where(t => t.Score > 0).OrderByDescending(o => o.Score).ToList();

            vm.Venues = _context.Venue.ToList();
            return View(vm);
        }


        public IActionResult Error()
        {
            return View();
        }
    }
}
