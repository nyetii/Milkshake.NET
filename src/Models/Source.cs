﻿using System.ComponentModel.DataAnnotations;
using System.Drawing;
using Milkshake.Models.Interfaces;

namespace Milkshake.Models;

public class Source : IMilkshake, IMedia<Source>
{
    public Guid Id { get; init; }

    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; set; }

    public string Name { get; set; } = null!;
    public string Description { get; set; } = string.Empty;
    public Size Size { get; init; }

    [Required(AllowEmptyStrings = false)]
    public string FileName { get; set; } = null!;

    public Task<Source> LoadAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Source> LoadAsync(Guid guid)
    {
        throw new NotImplementedException();
    }

    public Task<bool> SaveAsync(MemoryStream stream)
    {
        throw new NotImplementedException();
    }

    public Task<bool> RenameAsync(string newName)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteAsync()
    {
        throw new NotImplementedException();
    }
}