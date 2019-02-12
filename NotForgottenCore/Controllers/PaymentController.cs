using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NotForgottenCore.Data;
using NotForgottenCore.Models;

namespace NotForgottenCore.Controllers
{

    public class PaymentController : Controller
    {

        private SignInManager<ApplicationUser> _signManager;
        private UserManager<ApplicationUser> _userManager;

        public PaymentController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signManager)
        {
            _userManager = userManager;
            _signManager = signManager;
        }

        public IActionResult Pay([FromBody] stripeTokenPlaceholder stripeTokenP)
        {
            StripeCC stripe = new StripeCC(2500, stripeTokenP.stripeToken);

            return StatusCode(202);
            //return RedirectToAction("Race", "Races");
        }
    }
}