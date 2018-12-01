using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotForgottenCore.Models
{
    public class Table : BaseModel<int>
    {
        //Calculated via Database Trigger
        public int OpenSeats { get; set; } = 10;
        public List<Group> Groups { get; set; }
    }
}
