using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Milkshake.Models.Interfaces
{
    public interface ISource : IMedia
    {
        public Guid MilkshakeContextId { get; set; }
    }
}
