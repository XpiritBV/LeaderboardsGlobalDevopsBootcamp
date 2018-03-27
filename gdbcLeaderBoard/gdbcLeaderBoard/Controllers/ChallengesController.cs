using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using gdbcLeaderBoard.Data;
using gdbcLeaderBoard.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;

namespace gdbcLeaderBoard.Controllers
{
    [Authorize(Roles = "Xpirit")]
    public class ChallengesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private string secret;

        public ChallengesController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: Challenges
        public async Task<IActionResult> Index()
        {
            return View(await _context.Challenge.ToListAsync());
        }

        // GET: Challenges/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var challenge = await _context.Challenge
                .SingleOrDefaultAsync(m => m.Id == id);
            if (challenge == null)
            {
                return NotFound();
            }

            return View(challenge);
        }

        // GET: Challenges/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpGet("~/ChallengeCompleted")]
        [AllowAnonymous]
        public async Task<IActionResult> SubmitChallengeComplete(string challangename, string teamname, string venuename, string secret, bool helpused = false)
        {
            if (secret != _configuration.GetValue<string>("Token"))
            {
                return Unauthorized();
            }

            var challange = await _context.Challenge.SingleOrDefaultAsync(c => c.Name == challangename);
            if (challange == null)
            {
                return BadRequest("Unknown challange");
            }

            var team = await _context.Team.SingleOrDefaultAsync(c => c.Name == teamname);
            if (team == null)
            {
                var venue = await _context.Venue.SingleOrDefaultAsync(c => c.Name == venuename);
                if (venue == null)
                {
                    venue = new Venue()
                    {
                        Name = venuename
                    };
                    await _context.Venue.AddAsync(venue);
                }
                team = new Team()
                {
                    Venue = venue,
                    Name = teamname
                };
            }

            var tsi = await _context.TeamScoreItem.SingleOrDefaultAsync(t => t.Challenge == challange && t.Team == team);
        
            if (tsi == null)
            {
                tsi = new TeamScoreItem()
                {
                    Challenge = challange,
                    Team = team,
                    HelpUsed = helpused
                };
                await _context.TeamScoreItem.AddAsync(tsi);
            }
            else
            {
                tsi.HelpUsed = helpused;
            }
            await _context.SaveChangesAsync();
            return Accepted(tsi);
        }

        [HttpGet("~/ChallengeUnCompleted")]
        [AllowAnonymous]
        public async Task<IActionResult> UnSubmitChallengeComplete(string challangename, string teamname, string venuename, string secret)
        {
            if (secret != _configuration.GetValue<string>("Token"))
            {
                return Unauthorized();
            }

            var challange = await _context.Challenge.SingleOrDefaultAsync(c => c.Name == challangename);
            if (challange == null)
            {
                return BadRequest("Unknown challange");
            }

            var team = await _context.Team.SingleOrDefaultAsync(c => c.Name == teamname);
            if (team == null)
            {
                return BadRequest("Unknown team");
            }

            var tsi = await _context.TeamScoreItem.SingleOrDefaultAsync(t => t.Challenge == challange && t.Team == team);
            if (tsi == null)
            {
                return BadRequest("Team Score not found");
            }
            else
            {
                _context.TeamScoreItem.Remove(tsi);
            }
            await _context.SaveChangesAsync();
            return Accepted(tsi);
        }

        // POST: Challenges/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Points")] Challenge challenge)
        {
            if (ModelState.IsValid)
            {
                _context.Add(challenge);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(challenge);
        }

        // GET: Challenges/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var challenge = await _context.Challenge.SingleOrDefaultAsync(m => m.Id == id);
            if (challenge == null)
            {
                return NotFound();
            }
            return View(challenge);
        }

        // POST: Challenges/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Points")] Challenge challenge)
        {
            if (id != challenge.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(challenge);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChallengeExists(challenge.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            return View(challenge);
        }

        // GET: Challenges/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var challenge = await _context.Challenge
                .SingleOrDefaultAsync(m => m.Id == id);
            if (challenge == null)
            {
                return NotFound();
            }

            return View(challenge);
        }

        // POST: Challenges/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var challenge = await _context.Challenge.SingleOrDefaultAsync(m => m.Id == id);
            _context.Challenge.Remove(challenge);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool ChallengeExists(int id)
        {
            return _context.Challenge.Any(e => e.Id == id);
        }
    }
}
