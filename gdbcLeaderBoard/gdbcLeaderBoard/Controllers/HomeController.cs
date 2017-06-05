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
           
            var teamScores = _context.Team.Select(tt =>
                new TeamScoreViewModel { Venue = tt.Venue.Name, Team = tt.Name, Score = tt.Scores.Sum(s => s.Challenge.Points) }
            )
            .OrderByDescending(o => o.Score)
            .ToList();
            

            vm.TeamScores = teamScores;

            var venueScores = _context.Team.Select(t =>
                new VenueScoreViewModel { Venue = t.Venue.Name, Score = t.Scores.Sum(s => s.Challenge.Points) }
            )
            .GroupBy(x => x.Venue)
            .Select(x =>
                new VenueScoreViewModel { Venue = x.Key, Score = x.Sum(s => s.Score) }
            )
            .OrderByDescending(o => o.Score)
            .ToList();

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
