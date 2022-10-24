using Microsoft.EntityFrameworkCore;
using OpenBox.Domain.Entities;
using OpenBox.Domain.Tests.Unit.Fakers;

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

    public static IEnumerable<Product> Products => new ProductFaker().Generate(2);

    public void Dispose()
    {
        DbContext.Dispose();
    }
}