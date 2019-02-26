using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace NotForgottenCore.Models
{
    public class ApplicationUser : IdentityUser<Guid> 
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Zip { get; set; }
        [Required]
        public override string Email { get => base.Email; set => base.Email = value; }

        [NotMapped]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [NotMapped]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Confirm Password required")]
        [CompareAttribute("Password", ErrorMessage = "Password doesn't match.")]
        public string ConfirmPassword { get; set; }

        public int Balance { get; set; }
        public ICollection<Horse> Horses { get; set; }
        public ICollection<Group> Groups { get; set; }
        public ICollection<SingleTickets> SingleTickets { get; set; }
    }
}
