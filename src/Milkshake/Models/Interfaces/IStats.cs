using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Milkshake.Models.Interfaces
{
    public interface IStats
    {
        public Guid Id { get; set; }
        public DateTime CreationDateTime { get; set; }
        
        public string Name { get; set; }
        public string Description { get; set; }

        public Guid MilkshakeContextId { get; set; }

        public int TimesUsed { get; set; }
        public string Creator { get; set; }
    }
}
