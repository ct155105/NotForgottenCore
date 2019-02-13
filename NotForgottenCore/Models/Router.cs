using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotForgottenCore.Models
{
    public class Router
    {
        public string Action { get; set; }
        public string Controller { get; set; }
        public Dictionary<string, string> RouteValue { get; set; }

        public Router()
        {
            RouteValue = new Dictionary<string, string>();
        }
    }
}
