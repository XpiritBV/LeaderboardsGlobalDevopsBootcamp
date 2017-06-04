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

        public IActionResult Index()
        {
            ScoreOverviewViewModel vm = new ScoreOverviewViewModel();
            vm.VenueScores = new List<VenueScoreViewModel>();

            var teamScores = _context.Team.Include(t => t.Scores).Select(tt => 
                new TeamScoreViewModel{Venue = tt.Venue.Name, Team = tt.Name, Score = tt.Scores.Sum(s => s.Challenge.Points) }
            ).ToList();

            vm.TeamScores = teamScores;

            var venueScores = _context.Venue.Include(v => v.Teams).Select(vs =>
                new VenueScoreViewModel { Venue = vs.Name, Score = vs.Teams.Sum(s => s.Scores.Sum(sc => sc.Challenge.Points)) }
            ).ToList();

            vm.VenueScores = venueScores;

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
