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
        public Horse Horse { get; set; }

        public string GetStartTime()
        {
            var startTime = "";
            if (Id == 1)
            {
                startTime = "6:00PM";
            }
            if (Id == 2)
            {
                startTime = "6:15PM";
            }
            if (Id == 3)
            {
                startTime = "6:30PM";
            }
            if (Id == 4)
            {
                startTime = "6:45PM";
            }
            if (Id == 5)
            {
                startTime = "7:00PM";
            }
            if (Id == 6)
            {
                startTime = "7:15PM";
            }
            if (Id == 7)
            {
                startTime = "7:30PM";
            }
            if (Id == 8)
            {
                startTime = "7:45PM";
            }
            if (Id == 9)
            {
                startTime = "8:00PM";
            }
            if (Id == 10)
            {
                startTime = "8:15PM";
            }
            return startTime;
        }
    }
}
