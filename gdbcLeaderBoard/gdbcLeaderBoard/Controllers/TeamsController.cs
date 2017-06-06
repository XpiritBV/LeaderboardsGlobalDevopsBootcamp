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
    public class TeamsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TeamsController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: Teams
        public async Task<IActionResult> Index()
        {
            return View(await _context.Team.Include(t=> t.Venue).ToListAsync());
        }

        // GET: Teams/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var team = await _context.Team.Where(t=> t.Venue.VenueAdmin.UserName == this.User.Identity.Name || this.User.IsInRole("Xpirit"))
                .SingleOrDefaultAsync(m => m.Id == id);
            if (team == null)
            {
                return NotFound();
            }

            return View(team);
        }

        // GET: Teams/Create
        public IActionResult Create()
        {

            ViewData["Venues"] = GetVenues();

            return View();
        }

        private IEnumerable<SelectListItem> GetVenues()
        {
            var venues = _context.Venue.ToList();
            return new SelectList(venues, "Id", "Name");
        }

        // POST: Teams/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name","VenueID")] Team team)
        {
            if (ModelState.IsValid)
            {
                if (_context.Venue.Any(v =>( v.VenueAdmin.UserName == this.User.Identity.Name && v.Id == team.VenueID) || this.User.IsInRole("Xpirit")))
                {
                    _context.Add(team);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }
            return View(team);
        }

        // GET: Teams/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var team = await _context.Team.SingleOrDefaultAsync(m => m.Id == id);

            if (team == null)
            {
                return NotFound();
            }

            ViewData["Venues"] = GetVenues();
            return View(team);
        }

        // POST: Teams/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Team team)
        {
            if (id != team.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (_context.Venue.Any(v => (v.VenueAdmin.UserName == this.User.Identity.Name && v.Id == team.VenueID) || this.User.IsInRole("Xpirit")))
                    {
                        _context.Update(team);
                        await _context.SaveChangesAsync();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TeamExists(team.Id))
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
            return View(team);
        }

        // GET: Teams/Delete/5
        [Authorize("Xpirit")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var team = await _context.Team
                .SingleOrDefaultAsync(m => m.Id == id);
            if (team == null)
            {
                return NotFound();
            }

            return View(team);
        }

        // POST: Teams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize("Xpirit")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var team = await _context.Team.SingleOrDefaultAsync(m => m.Id == id);
            _context.Team.Remove(team);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool TeamExists(int id)
        {
            return _context.Team.Any(e => e.Id == id);
        }
    }
}
