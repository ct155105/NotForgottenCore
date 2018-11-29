using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotForgottenCore.Models
{
    public class Horse : BaseModel<Guid>
    {
        public string Name { get; set; }
        public string Trainer { get; set; }
        public Guid ApplicationUserId { get; set; }
        public ICollection<Race> Races { get; set; }

        public override void GetId()
        {
            Id = Guid.NewGuid();
        }
    }
}
