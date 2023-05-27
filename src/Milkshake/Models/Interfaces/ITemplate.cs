using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Milkshake.Models.Interfaces
{
    public interface ITemplate<TTopping> : IMedia where TTopping : class, ITopping
    {
        public ICollection<TTopping>? Toppings { get; set; }

        public Guid MilkshakeContextId { get; set; }
    }
}
