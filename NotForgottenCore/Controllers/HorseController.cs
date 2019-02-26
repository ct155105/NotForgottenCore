using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NotForgottenCore.Data;
using NotForgottenCore.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;

namespace NotForgottenCore.Controllers
{
    public class HorseController : Controller
    {
        private readonly ApplicationDataContext _context;
        private UserManager<ApplicationUser> _userManager;

        public HorseController(ApplicationDataContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Horses
        public async Task<IActionResult> Index()
        {
            return View(await _context.Horses.ToListAsync());
        }

        // GET: Horses/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var horse = await _context.Horses
                .Include(r => r.Owner)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (horse == null)
            {
                return NotFound();
            }

            return View(horse);
        }

        // GET: Horses/Create
        //[Authorize]
        public async Task<IActionResult> Create(int raceId, int laneId)
        {

            ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
            if (user == null)
            {
                //Store horse info in session for redirect after login

                //****CT***** Need to convert this to Router class object
                HttpContext.Session.SetString("redirect", "true");
                HttpContext.Session.SetString("controller", "Horse");
                HttpContext.Session.SetString("action", "Create");
                RouteValues obj_routeValues = new RouteValues { RaceId = raceId, LaneId = laneId};
                var routeValues = JsonConvert.SerializeObject(obj_routeValues);
                HttpContext.Session.SetString("routeValues", routeValues);
                return RedirectToAction("login","ApplicationUser");                
            }

            ViewData["raceId"] = raceId;
            ViewData["laneId"] = laneId;
            ViewData["year"] = DateTime.Now.Year;

            return View();
        }

        // POST: Horses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("Name,Trainer")] Horse horse, int raceId, int laneId, int year)
        {
            ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
            Race race = await _context.Races.FindAsync(raceId, laneId, year);

            if (ModelState.IsValid)
            {
                Guid id = Guid.NewGuid();
                horse.Id = id;
                horse.ApplicationUserId = user.Id;
                _context.Add(horse);
                try
                {
                    race.HorseId = id;
                    _context.Update(race);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HorseExists(horse.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                await _context.SaveChangesAsync();

                return RedirectToAction("Race", "Races", new { raceNbr = raceId });
            }
            return View(horse);
        }

        // GET: Horses/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var horse = await _context.Horses.FindAsync(id);
            if (horse == null)
            {
                return NotFound();
            }
            return View(horse);
        }

        // POST: Horses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Name,Trainer,ApplicationUserId,Id")] Horse horse)
        {
            if (id != horse.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(horse);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HorseExists(horse.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(horse);
        }

        // GET: Horses/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var horse = await _context.Horses
                .FirstOrDefaultAsync(m => m.Id == id);
            if (horse == null)
            {
                return NotFound();
            }

            return View(horse);
        }

        // POST: Horses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var horse = await _context.Horses.FindAsync(id);
            _context.Horses.Remove(horse);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HorseExists(Guid id)
        {
            return _context.Horses.Any(e => e.Id == id);
        }
    }
}
