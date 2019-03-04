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
            HttpContext.Session.SetString("redirect", "true");
            HttpContext.Session.SetString("action", "TableApp");
            HttpContext.Session.SetString("controller", "Tables");

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
            string ownerName = user.FirstName + " " + user.LastName;

            for (int i = nbrSeats; i > 0; i--)
            {
                GroupMember member = new GroupMember
                {
                    Id = Guid.NewGuid(),
                    Name = ownerName,
                    GroupId = groupId
                };
                members.Add(member);
                ownerName = "";
            }

            Group group = new Group
            {
                GroupName = user.LastName + " Group",
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
                return RedirectToAction("UserCart", "ApplicationUser");
            }
            return View(group);
        }

        // GET: Groups/Create
        [HttpGet("/Groups/DisplayPartial")]
        public IActionResult _GroupDisplayPartial(Group group)
        {
            return PartialView(group);
        }

        // GET: Tables/_SingleTicketParentPartial
        [HttpGet]
        public async Task<IActionResult> _SingleTicketParentPartial(int tickets)
        {
            SingleTicketsCollection singleTicketsCollection = new SingleTicketsCollection();
            ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
            string name = user.FirstName + " " + user.LastName;

            while (tickets > 0)
            {
                SingleTickets ticket = new SingleTickets()
                {
                    Id = Guid.NewGuid() ,
                    Name = name
                };
                singleTicketsCollection.SingleTickets.Add(ticket);
                name = "";
                tickets--;
            }
            return PartialView(singleTicketsCollection);
        }

        [HttpPost("/Tables/_SingleTicketCreate")]
        public async Task<IActionResult> _SingleTicketCreate(SingleTicketsCollection tickets)
        {
            ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);

            foreach(var ticket in tickets.SingleTickets)
            {

                if (ModelState.IsValid)
                {
                    ticket.ApplicationUserId = user.Id;
                    _context.Add(ticket);
                    await _context.SaveChangesAsync();
                }
            }
            return RedirectToAction("UserCart", "ApplicationUser");
        }

        // GET: Tables/_SingleTicketPartial
        public IActionResult _SingleTicketPartial()
        {
            return PartialView();
        }

    }
}