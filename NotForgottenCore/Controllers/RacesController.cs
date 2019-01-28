using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NotForgottenCore.Data;
using NotForgottenCore.Models;

namespace NotForgottenCore.Controllers
{
    public class RacesController : Controller
    {
        private readonly ApplicationDataContext _context;

        public RacesController(ApplicationDataContext context)
        {
            _context = context;
        }

        // GET: Races
        [Route("Races")]
        public IActionResult Race()
        {
            int id = 1;
            List<Race> races = (from c in _context.Races
                                .Include(r => r.Horse)
                                    .ThenInclude(horse => horse.Owner)
                                where c.Id == id
                                select c
                                ).ToList();

            if (races == null)
            {
                return NotFound();
            }

            return View(races);
        }

        //Need to add filter for year
        public IActionResult _LanesPartial(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            List<Race> races = (from c in _context.Races
                    .Include(r => r.Horse)
                        .ThenInclude(horse => horse.Owner)
                                where c.Id == id
                                select c
                    ).ToList();

            if (races == null)
            {
                return NotFound();
            }

            return PartialView(races);
        }

    }
}
