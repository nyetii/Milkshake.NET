using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Milkshake.Models.Interfaces;

namespace Milkshake.Models
{
    public class Template : ITemplate<TemplateProperties>, IStats
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
        public ICollection<TemplateProperties>? Properties { get; set; } = new List<TemplateProperties>();

        public Guid MilkshakeContextId { get; set; }
        public Instance Milkshake { get; set; } = null!;

        public int TimesUsed { get; set; } = 0;
        public string Creator { get; set; } = "Unknown";
    }
}
