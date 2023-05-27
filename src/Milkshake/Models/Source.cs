using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Milkshake.Models.Interfaces;

namespace Milkshake.Models
{
    /// <summary>
    /// Represents the image that will be randomly selected and its metadata used on a <see cref="Topping"/>.
    /// </summary>
    public class Source : ISource, IStats
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime CreationDateTime { get; set; } = DateTime.Now;

        [MaxLength(32)]
        public string Name { get; set; } = "Unknown";

        [MaxLength(64)]
        public string Description { get; set; } = "No description provided.";
        public string Path { get; set; } = string.Empty;
        public int Width { get; set; }
        public int Height { get; set; }
        public ImageTags Tags { get; set; } = ImageTags.Any;

        public Guid MilkshakeContextId { get; set; }
        public Instance Milkshake { get; set; } = null!;

        public int TimesUsed { get; set; } = 0;
        public string Creator { get; set; } = "Unknown";
    }
}
