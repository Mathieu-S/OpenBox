using System.ComponentModel.DataAnnotations;
using OpenBox.Domain.Common;

namespace OpenBox.Domain.Entities;

public record Brand : Entity
{
    [MaxLength(255)] public string Name { get; set; } = null!;
}