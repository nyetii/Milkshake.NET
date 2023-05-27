using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ImageMagick;
using Milkshake.Models.Interfaces;
using ArgumentOutOfRangeException = System.ArgumentOutOfRangeException;

namespace Milkshake.Models
{
    /// <summary>
    /// Represents each set of properties on <see cref="Models.Template"/> that a <see cref="Source"/> must meet on generation.
    /// </summary>
    public class Topping : ITopping
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [MaxLength(32)]
        public string Name { get; set; }

        [MaxLength(64)] 
        public string Description { get; set; } = "No description provided.";
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public Layer Layer { get; set; }
        public bool IsText { get; set; } = false;
        public string Color { get; set; } = "#000000";
        public Gravity Orientation { get; set; } = Gravity.West;
        public string Font { get; set; } = "Arial";
        public string StrokeColor { get; set; } = "#000000";
        public int StrokeWidth { get; set; } = 0;

        public ImageTags Tags { get; set; } = ImageTags.Any;

        public Guid TemplateId { get; set; }

        public Template Template { get; set; } = null!;
        [NotMapped]
        private int _index = 1;
        public int Index { get => _index; set => _index = value is < 1 or > 16 ? throw new ArgumentOutOfRangeException() : value; }
    }
}
