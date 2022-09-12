using System.ComponentModel.DataAnnotations;
using OpenBox.Domain.Common;

namespace OpenBox.Domain.Entities;

public record Product : Entity
{
    [MaxLength(255)] public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public uint Price { get; set; }
    
    public Guid BrandId { get; set; }
    public Brand Brand { get; set; } = null!;
}