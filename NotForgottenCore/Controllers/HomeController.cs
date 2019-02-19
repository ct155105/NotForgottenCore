using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NotForgottenCore.Data;

namespace NotForgottenCore.Controllers
{
    
    public class HomeController : Controller
    {
        private ApplicationDataContext _dataContext;

        public HomeController(ApplicationDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [Route("/")]
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [Route("/About")]
        [HttpGet]
        public IActionResult About()
        {
            return View();
        }

        [Route("/Contact")]
        [HttpGet]
        public IActionResult Contact()
        {
            return View();
        }

        [HttpGet]
        public IActionResult NATR2019()
        {
            return View();
        }
    }
}