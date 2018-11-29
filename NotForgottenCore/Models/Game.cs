using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotForgottenCore.Models
{
    public class Game : BaseModel<Guid>
    {
        public string Name { get; set; }
        public string Rules { get; set; }
    }
}
