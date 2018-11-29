using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NotForgottenCore.Models
{
    public abstract class BaseModel<T>
    {
        public T Id { get; set; }

        public virtual void GetId() { }
    }
}
