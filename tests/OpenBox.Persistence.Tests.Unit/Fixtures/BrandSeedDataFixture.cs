using Microsoft.EntityFrameworkCore;
using OpenBox.Domain.Entities;
using OpenBox.Domain.Tests.Unit.Fakers;

namespace OpenBox.Persistence.Tests.Unit.Fixtures;

public class BrandSeedDataFixture : IDisposable
{
    public BrandSeedDataFixture()
    {
        var options = new DbContextOptionsBuilder<OpenBoxDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        DbContext = new OpenBoxDbContext(options);
        DbContext.Database.EnsureCreated();
        DbContext.AddRange(Brands);
        DbContext.SaveChanges();
    }

    public OpenBoxDbContext DbContext { get; }

    public static IEnumerable<Brand> Brands => new BrandFaker().Generate(2);

    public void Dispose()
    {
        DbContext.Dispose();
    }
}