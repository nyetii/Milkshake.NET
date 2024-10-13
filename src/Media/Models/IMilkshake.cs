using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace Milkshake.Media.Models;

public interface IMilkshake
{
    public Guid Id { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; set; }

    [Required(AllowEmptyStrings = false)]
    [MaxLength(64)]
    public string Name { get; set; }
    public string Description { get; set; }

    public Size Size { get; init; }
}