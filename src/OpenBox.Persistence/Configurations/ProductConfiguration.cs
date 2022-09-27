using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OpenBox.Domain.Entities;

namespace OpenBox.Persistence.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        // Entity column specification
        builder
            .HasOne(x => x.Brand)
            .WithMany();
    }
}