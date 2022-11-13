using Microsoft.EntityFrameworkCore;

namespace OpenBox.Persistence.Tests.Unit.Fixtures;

public class BrandSeedDataFixture : IDisposable
{
    public OpenBoxDbContext DbContext { get; }

    public BrandSeedDataFixture()
    {
        var options = new DbContextOptionsBuilder<OpenBoxDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        DbContext = new OpenBoxDbContext(options);
        Seed();
    }

    private void Seed()
    {
        DbContext.Database.EnsureCreated();
    }

    public void Dispose()
    {
        DbContext.Dispose();
    }
}