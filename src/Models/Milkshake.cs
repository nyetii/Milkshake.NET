using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using Milkshake.Models.Interfaces;

namespace Milkshake.Models;

public class Milkshake : IMilkshake
{
    public Guid Id { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; set; }

    public string Name { get; set; } = null!;
    public string Description { get; set; } = string.Empty;

    public Size Size { get; init; }
}