using System.Drawing;
using System.Text.Json.Serialization;

namespace Milkshake.Media;

public class Source : Media, IMilkshake
{
    [JsonIgnore]
    public Guid Id { get; init; }

    public string Name { get; set; } = null!;
    public string Description { get; set; } = string.Empty;
    public Size Size { get; set; }

    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; set; }
}