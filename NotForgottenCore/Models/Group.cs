using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotForgottenCore.Models
{
    public class Group : BaseModel<Guid>
    {
        public string GroupName { get; set; }
        public List<GroupMember> Members { get; set; }
        public Guid ApplicationUserId { get; set; }
        public int TableId { get; set; }
        public int NumberSeats { get; set; }
    }
}
