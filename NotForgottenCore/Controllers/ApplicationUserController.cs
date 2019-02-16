using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NotForgottenCore.Data;
using NotForgottenCore.Models;

namespace NotForgottenCore.Controllers
{
    public class ApplicationUserController : Controller
    {
        private SignInManager<ApplicationUser> _signManager;
        private UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDataContext _context;

        public ApplicationUserController(ApplicationDataContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signManager)
        {
            _userManager = userManager;
            _signManager = signManager;
            _context = context;
        }

        // GET: UserInfo
        [Route("/userinfo")]
        public async Task<IActionResult> UserInfo()
        {
            return View(await _context.Users
                .OrderByDescending(x => x.Balance)
                .ToListAsync());
        }

        [HttpGet("/Register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost("/Register")]
        public async Task<IActionResult> Register(ApplicationUser model)
        {

            model.UserName = model.Email;

            if (ModelState.IsValid)
            {
                var result = await _userManager.CreateAsync(model, model.Password);

                if (result.Succeeded)
                {
                    await _signManager.SignInAsync(model, false);


                    //Check if user was trying to do an action prior to being redirected to login page
                    if (!string.IsNullOrEmpty(HttpContext.Session.GetString("redirect")))
                    {
                        if (!string.IsNullOrEmpty(HttpContext.Session.GetString("routeValues")))
                        {
                            var routeValues = JsonConvert.DeserializeObject<RouteValues>(HttpContext.Session.GetString("routeValues"));
                            HttpContext.Session.SetString("routeValues", "");
                            return RedirectToAction(HttpContext.Session.GetString("action")
                            , HttpContext.Session.GetString("controller")
                            , routeValues);
                        }
                        return RedirectToAction(HttpContext.Session.GetString("action")
                        , HttpContext.Session.GetString("controller"));
                    }

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return View();
        }

        [HttpGet("/Login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost("/Login")]
        public async Task<IActionResult> Login(ApplicationUser model)
        {

            model.UserName = model.Email;

            var result = await _signManager.PasswordSignInAsync(model.UserName, model.Password, false, false);

            if (result.Succeeded)
            {
                //Check if user was trying to do an action prior to being redirected to login page
                if (!string.IsNullOrEmpty(HttpContext.Session.GetString("redirect")))
                {
                    if (!string.IsNullOrEmpty(HttpContext.Session.GetString("routeValues")))
                    {
                        var routeValues = JsonConvert.DeserializeObject<RouteValues>(HttpContext.Session.GetString("routeValues"));
                        HttpContext.Session.SetString("routeValues", "");
                        return RedirectToAction(HttpContext.Session.GetString("action")
                        , HttpContext.Session.GetString("controller")
                        , routeValues);
                    }
                    return RedirectToAction(HttpContext.Session.GetString("action")
                    , HttpContext.Session.GetString("controller"));
                }

                return RedirectToAction("Index", "Home");
            }

            ViewData["LoginFail"] = "Login Failed. Please try again.";
            return View();
        }

        [HttpGet("/Logout")]
        public async Task<IActionResult> Logout()
        {
            await _signManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}