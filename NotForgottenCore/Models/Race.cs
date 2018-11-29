using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotForgottenCore.Models
{
    public class Race : BaseModel<int>
    {
        public int Year { get; set; }
        public int LaneId { get; set; }
        public Guid? HorseId { get; set; }
    }
}
