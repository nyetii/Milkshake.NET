using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Milkshake.Attributes;
using Milkshake.Models.Interfaces;

namespace Milkshake.Models
{
    
    public class Instance : IInstance<Template, Source, TemplateProperties>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ContextId { get; set; } = Guid.NewGuid();
        
        public string? Vips { get; set; }
        public ICollection<Template>? Template { get; set; } = new List<Template>();
        public ICollection<Source>? Source { get; set; } = new List<Source>();
    }
}
