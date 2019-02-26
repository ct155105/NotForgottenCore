using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotForgottenCore.Models
{
    public class SingleTicketsCollection
    {
        public List<SingleTickets> SingleTickets { get; set; }

        public SingleTicketsCollection()
        {
            SingleTickets = new List<SingleTickets>();
        }
    }

    public class SingleTickets : BaseModel<Guid>
    {
        public Guid ApplicationUserId { get; set; }
        public string Name { get; set; }
    }
}
