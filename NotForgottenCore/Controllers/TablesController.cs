using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NotForgottenCore.Data;
using NotForgottenCore.Models;

namespace NotForgottenCore.Controllers
{
    public class TablesController : Controller
    {
        private readonly ApplicationDataContext _context;
        private UserManager<ApplicationUser> _userManager;

        public TablesController(ApplicationDataContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet("/Tables")]
        public async Task<IActionResult> TableApp()
        {
            return View(await _context.Tables.ToListAsync());
        }

        public async Task<IActionResult> _TablePartial(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            HttpContext.Session.SetString("redirect", "true");
            HttpContext.Session.SetString("action", "TableApp");
            HttpContext.Session.SetString("controller", "Tables");

            Table table = await _context.Tables
                .Include(t => t.Groups)
                .ThenInclude(g => g.Members)
            .FirstOrDefaultAsync(m => m.Id == id);

            if (table == null)
            {
                return NotFound();
            }

            return PartialView(table);
        }

        // GET: Groups/Create
        [HttpGet("/Groups/CreatePartial")]
        public async Task<IActionResult> _GroupCreate(int nbrSeats, int tableId)
        {
            ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
            List<GroupMember> members = new List<GroupMember>();
            Guid groupId = Guid.NewGuid();

            for (int i = nbrSeats; i > 0; i--)
            {
                GroupMember member = new GroupMember
                {
                    Id = Guid.NewGuid(),
                    GroupId = groupId
                };
                members.Add(member);
            }

            Group group = new Group
            {
                Id = groupId,
                NumberSeats = nbrSeats,
                TableId = tableId,
                Members = members
            };
            return PartialView(group);
        }

        [HttpPost("/Groups/CreatePartial")]
        public async Task<IActionResult> _GroupCreate(Group group)
        {
            ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);

            if (ModelState.IsValid)
            {
                group.ApplicationUserId = user.Id;
                _context.Add(group);
                //foreach (var member in group.Members)
                //{
                //    member.GroupId = group.Id;
                //    _context.Add(member);
                //}
                await _context.SaveChangesAsync();
                return RedirectToAction("TableApp");
            }
            return View(group);
        }

        // GET: Groups/Create
        [HttpGet("/Groups/DisplayPartial")]
        public IActionResult _GroupDisplayPartial(Group group)
        {
            return PartialView(group);
        }

    }
}