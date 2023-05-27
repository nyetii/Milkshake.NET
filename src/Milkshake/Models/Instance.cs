using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Milkshake.Attributes;
using Milkshake.Models.Interfaces;

namespace Milkshake.Models
{
    /// <summary>
    /// Represents an Instance of the Milkshake library.
    /// Each Instance contains its own scope where the Milkshake objects are enclosed in.
    /// </summary>
    public class Instance : IInstance<Template, Source, Topping>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ContextId { get; set; } = Guid.NewGuid();
        
        public string? Vips { get; set; }
        public ICollection<Template>? Template { get; set; } = new List<Template>();
        public ICollection<Source>? Source { get; set; } = new List<Source>();
    }
}
