using Microsoft.EntityFrameworkCore;
using OpenBox.Domain.Entities;

namespace OpenBox.Persistence.Tests.Unit.Fixtures;

public class ProductSeedDataFixture : IDisposable
{
    public ProductSeedDataFixture()
    {
        var options = new DbContextOptionsBuilder<OpenBoxDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        DbContext = new OpenBoxDbContext(options);
        DbContext.Database.EnsureCreated();
        DbContext.AddRange(Products);
        DbContext.SaveChanges();
    }

    public OpenBoxDbContext DbContext { get; }

    public static IEnumerable<Product> Products =>
        new List<Product>
        {
            new()
            {
                Id = Guid.NewGuid(), Name = "Bob", Description = "", Price = 10,
                Brand = new Brand { Id = Guid.NewGuid(), Name = Faker.Company.Name() }
            },
            new()
            {
                Id = Guid.NewGuid(), Name = "Bip", Description = "testy", Price = 12,
                Brand = new Brand { Id = Guid.NewGuid(), Name = Faker.Company.Name() }
            },
        };

    public void Dispose()
    {
        DbContext.Dispose();
    }
}