using System.Drawing;

namespace Milkshake.Media;

public class Milkshake : IMilkshake
{
    public Guid Id { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; set; }

    public string Name { get; set; } = null!;
    public string Description { get; set; } = string.Empty;

    public Size Size { get; init; }
}