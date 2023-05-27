using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Milkshake.Models.Interfaces;

namespace Milkshake.Models
{
    /// <summary>
    /// Represents the image that will serve as a base for the generated Milkshake.
    /// </summary>
    public class Template : ITemplate<Topping>, IStats
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime CreationDateTime { get; set; } = DateTime.Now;
        [MaxLength(32)]
        public string Name { get; set; }

        [MaxLength(64)] 
        public string Description { get; set; } = "No description provided.";
        public string Path { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public ImageTags Tags { get; set; } = ImageTags.Any;
        public ICollection<Topping>? Toppings { get; set; } = new List<Topping>();

        public Guid MilkshakeContextId { get; set; }
        public Instance Milkshake { get; set; } = null!;

        public int TimesUsed { get; set; } = 0;
        public string Creator { get; set; } = "Unknown";
    }
}
