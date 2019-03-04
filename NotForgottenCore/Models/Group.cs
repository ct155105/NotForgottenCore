using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NotForgottenCore.Models
{
    public class Group : BaseModel<Guid>
    {
        [Required]
        public string GroupName { get; set; }
        public List<GroupMember> Members { get; set; }
        public Guid ApplicationUserId { get; set; }
        public int TableId { get; set; }
        public int NumberSeats { get; set; }

        public int GetPrice()
        {
            int price = NumberSeats * 2000;
            if (NumberSeats == 8)
            {
                price = 24000;
            }
            if (NumberSeats == 10)
            {
                price = 28000;
            }
            if (NumberSeats == 15)
            {
                price = 40000;
            }
            if (NumberSeats == 20)
            {
                price = 50000;
            }
            return price;
        }
    }

}
