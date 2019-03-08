using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using NotForgottenCore.Data;
using NotForgottenCore.Models;

namespace NotForgottenCore.Controllers
{

    public class PaymentController : Controller
    {

        private UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDataContext _context;

        public PaymentController(UserManager<ApplicationUser> userManager, ApplicationDataContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<IActionResult> Pay([FromBody] stripeTokenPlaceholder stripeTokenP)
        {
            ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);

            StripeCC stripe = new StripeCC(stripeTokenP.Amount, stripeTokenP.StripeToken, user.Email, stripeTokenP.Description);

            var data = new { };

            return Ok(data);
        }

        public async Task<IActionResult> PayCash([FromBody] RouteValuesAmount amount)
        {
            ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);

            //***CT***** Can convert to JSON -> Dictionary to avoid strongly type class
            //var json = JsonConvert.SerializeObject(amount);
            //var dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            //int balance = Convert.ToInt32(dictionary.GetValueOrDefault("Amount"));
            //user.Balance += balance;

            user.Balance += amount.Amount;
            _context.Update(user);
            await _context.SaveChangesAsync();

            return Ok(user);
        }
    }
}