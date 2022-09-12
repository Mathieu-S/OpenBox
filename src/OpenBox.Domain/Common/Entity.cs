using System.ComponentModel.DataAnnotations;

namespace OpenBox.Domain.Common;

public abstract record Entity
{
    [Key] public Guid Id { get; set; }
}