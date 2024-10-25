using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Text.Json.Serialization;

namespace Milkshake.Media;

public interface IMilkshake
{
    [JsonIgnore]
    public Guid Id { get; init; }

    [Required(AllowEmptyStrings = false)]
    [MaxLength(64)]
    public string Name { get; set; }
    public string Description { get; set; }

    public Size Size { get; set; }

    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; set; }
}