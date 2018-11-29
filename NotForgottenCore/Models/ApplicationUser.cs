using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotForgottenCore.Models
{
    public class ApplicationUser : IdentityUser<Guid> 
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Zip { get; set; }
        public ICollection<Horse> Horses { get; set; }
        public ICollection<Group> Groups { get; set; }
    }
}
