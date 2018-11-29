using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NotForgottenCore.Data;

namespace NotForgottenCore.Controllers
{
    [Route("/")]
    public class HomeController : Controller
    {
        private ApplicationDataContext _dataContext;

        public HomeController(ApplicationDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return Ok("hello from home index");
        }
    }
}