using Microsoft.EntityFrameworkCore;
using OpenBox.Domain.Entities;
using OpenBox.Persistence.Configurations;

namespace OpenBox.Persistence;

public sealed class OpenBoxDbContext : DbContext
{
    public OpenBoxDbContext()
    {
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
    }

    public OpenBoxDbContext(DbContextOptions options) : base(options)
    {
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
    }

    public DbSet<Brand> Brands => Set<Brand>();
    public DbSet<Product> Products => Set<Product>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new BrandConfiguration());
        modelBuilder.ApplyConfiguration(new ProductConfiguration());
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Is only false when used with the EFCore CLI.
        // Used only for made a migration. This connection string is useless in production.
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Database=openbox;Username=admin;Password=admin");
        }
    }
}