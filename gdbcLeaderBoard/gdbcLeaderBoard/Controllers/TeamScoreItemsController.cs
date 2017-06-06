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

namespace gdbcLeaderBoard.Controllers
{
    [Authorize(Roles = "Xpirit,Venue")]
    public class TeamScoreItemsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TeamScoreItemsController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: TeamScoreItems
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.TeamScoreItem.Include(t => t.Challenge).Include(t => t.Team);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: TeamScoreItems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teamScoreItem = await _context.TeamScoreItem
                .Include(t => t.Challenge)
                .Include(t => t.Team)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (teamScoreItem == null)
            {
                return NotFound();
            }

            return View(teamScoreItem);
        }

        // GET: TeamScoreItems/Create
        public IActionResult Create()
        {
            ViewData["ChallengeID"] = new SelectList(_context.Challenge, "Id", "Name");
            if (this.User.IsInRole("Xpirit")) { 
            ViewData["TeamID"] = new SelectList(_context.Team.Select(t => new { Id = t.Id, Name = t.Venue.Name + " : " + t.Name }).OrderBy(t=> t.Name), "Id", "Name");
            }
            else
            {
                ViewData["TeamID"] = new SelectList(_context.Team.Where(t=> t.Venue.VenueAdmin.UserName == this.User.Identity.Name).Select(t => new { Id = t.Id, Name = t.Venue.Name + " : " + t.Name }).OrderBy(t => t.Name), "Id", "Name");
            }
            return View();
        }

        // POST: TeamScoreItems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ChallengeID,TeamID")] TeamScoreItem teamScoreItem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(teamScoreItem);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewData["ChallengeID"] = new SelectList(_context.Challenge, "Id", "Name", teamScoreItem.ChallengeID);
            ViewData["TeamID"] = new SelectList(_context.Team, "Id", "Name", teamScoreItem.TeamID);
            return View(teamScoreItem);
        }

        // GET: TeamScoreItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teamScoreItem = await _context.TeamScoreItem.SingleOrDefaultAsync(m => m.Id == id);
            if (teamScoreItem == null)
            {
                return NotFound();
            }
            ViewData["ChallengeID"] = new SelectList(_context.Challenge, "Id", "Name", teamScoreItem.ChallengeID);
            ViewData["TeamID"] = new SelectList(_context.Team, "Id", "Name", teamScoreItem.TeamID);
            return View(teamScoreItem);
        }

        // POST: TeamScoreItems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ChallengeID,TeamID")] TeamScoreItem teamScoreItem)
        {
            if (id != teamScoreItem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(teamScoreItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TeamScoreItemExists(teamScoreItem.Id))
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
            ViewData["ChallengeID"] = new SelectList(_context.Challenge, "Id", "Name", teamScoreItem.ChallengeID);
            ViewData["TeamID"] = new SelectList(_context.Team, "Id", "Name", teamScoreItem.TeamID);
            return View(teamScoreItem);
        }

        // GET: TeamScoreItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teamScoreItem = await _context.TeamScoreItem
                .Include(t => t.Challenge)
                .Include(t => t.Team)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (teamScoreItem == null)
            {
                return NotFound();
            }

            return View(teamScoreItem);
        }

        // POST: TeamScoreItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var teamScoreItem = await _context.TeamScoreItem.SingleOrDefaultAsync(m => m.Id == id);
            _context.TeamScoreItem.Remove(teamScoreItem);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool TeamScoreItemExists(int id)
        {
            return _context.TeamScoreItem.Any(e => e.Id == id);
        }
    }
}
