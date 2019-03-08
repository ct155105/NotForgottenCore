using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
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
        private readonly IEmailSender _emailSender;

        public ApplicationUserController(ApplicationDataContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signManager, IEmailSender emailSender)
        {
            _userManager = userManager;
            _signManager = signManager;
            _context = context;
            _emailSender = emailSender;
        }

        // GET: UserInfo
        [Route("/userinfo")]
        public async Task<IActionResult> UserInfo()
        {
            return View(await _context.Users
                .OrderByDescending(x => x.Balance)
                .ToListAsync());
        }

        [Route("/Cart")]
        public async Task<IActionResult> UserCart()
        {
            ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);

            ApplicationUser cartUser = await _context.ApplicationUser
                .Include(t => t.Groups)
                .ThenInclude(g => g.Members)
                .Include(o => o.Horses)
                .ThenInclude(h => h.Races)
                .Include(t => t.SingleTickets)
                .FirstOrDefaultAsync(m => m.Id == user.Id);

            return View(cartUser);
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

        [HttpGet("/ForgotPassword")]
        public IActionResult ForgotPassword()
        {
            ViewData["NoEmail"] = null;
            return View();
        }

        [HttpGet("/ForgotPasswordConfirmation")]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        [HttpPost("/ForgotPassword")]
        public async Task<IActionResult> ForgotPassword(ForgottenPassword input)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(input.Email);
                if (user == null)// || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    ViewData["NoEmail"] = "No account for that email address exists";
                    return View();
                }

                // For more information on how to enable account confirmation and password reset please 
                // visit https://go.microsoft.com/fwlink/?LinkID=532713
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                code = WebUtility.UrlEncode(code);
                var callbackUrl = $"{this.Request.Scheme}://{this.Request.Host}/ResetPassword?code={code}";
                //var callbackUrl = Url.Action("ResetPassword", "ApplicationUser", new { code });

                await _emailSender.SendEmailAsync(
                        input.Email,
                        "Reset Password",
                        $"Please reset your password by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                return Redirect("/ForgotPasswordConfirmation");
            }

            return View();
        }

        [HttpGet("/ResetPassword")]
        public IActionResult ResetPassword(string code = null)
        {
            if (code == null)
            {
                return BadRequest("A code must be supplied for password reset.");
            }
            else
            {
                ResetPassword password = new ResetPassword()
                {
                    Code = code
                };
                return View(password);
            }
        }

        [HttpPost("/ResetPassword")]
        public async Task<IActionResult> ResetPassword(ResetPassword input)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var user = await _userManager.FindByEmailAsync(input.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return Redirect("/Login");
            }

            var result = await _userManager.ResetPasswordAsync(user, input.Code, input.Password);
            if (result.Succeeded)
            {
                return Redirect("/Login");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return Redirect("/Login");
        }
    }
}