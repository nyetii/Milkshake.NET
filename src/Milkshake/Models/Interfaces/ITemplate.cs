using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Milkshake.Models.Interfaces
{
    public interface ITemplate<TProperties> : IMedia where TProperties : class, IProperties
    {
        public ICollection<TProperties>? Properties { get; set; }

        public Guid MilkshakeContextId { get; set; }
    }
}
