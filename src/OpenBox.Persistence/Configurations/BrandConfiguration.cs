using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OpenBox.Domain.Entities;

namespace OpenBox.Persistence.Configurations;

public class BrandConfiguration : IEntityTypeConfiguration<Brand>
{
    public void Configure(EntityTypeBuilder<Brand> builder)
    {
        // Entity column specification
        builder
            .HasIndex(x => x.Name)
            .IsUnique();

        // Seed
        builder
            .HasData(
                new Brand
                {
                    Id = Guid.NewGuid(),
                    Name = "Acme"
                }
            );
    }
}