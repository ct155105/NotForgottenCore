using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NotForgottenCore.Models
{
    public class GroupMember : BaseModel<Guid>
    {
        public Guid GroupId { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
