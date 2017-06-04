using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using gdbcLeaderBoard.Data;
using gdbcLeaderBoard.Models.HomeViewModels;

namespace gdbcLeaderBoard.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            ScoreOverviewViewModel vm = new ScoreOverviewViewModel();
            vm.TeamScores = new List<TeamScoreViewModel>();
            vm.VenueScores = new List<VenueScoreViewModel>();

            var teamscores = _context.Team.OrderByDescending(t => t.Scores.Sum(s => s.Challenge.Points)).Take(10);
            foreach(var team in teamscores)
            {
                if (team.Scores != null)
                {
                    vm.TeamScores.Add(new TeamScoreViewModel() { Team = team.Name, Score = team.Scores.Sum(s => s.Challenge.Points) });
                }
            }

            var venueScores = _context.Venue.OrderByDescending(v => v.Teams.Sum(t => t.Scores.Sum(s => s.Challenge.Points)));
            foreach (var venue in venueScores)
            {
                if (venue.Teams != null)
                {
                    vm.VenueScores.Add(new VenueScoreViewModel() { Venue = venue.Name, Score = venue.Teams.Sum(t => t.Scores.Sum(s => s.Challenge.Points)) });
                }
            }

            return View(vm);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
