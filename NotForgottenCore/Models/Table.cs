using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotForgottenCore.Models
{
    public class Table : BaseModel<int>
    {
        public int? OpenSeats { get; set; }
        public ICollection<Group> Groups { get; set; }
    }
}
