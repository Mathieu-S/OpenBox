using OpenBox.Application.Repositories;
using OpenBox.Domain.Tests.Unit.Fakers;
using OpenBox.Persistence.Repositories;
using OpenBox.Persistence.Tests.Unit.Fixtures;
using Xunit;

namespace OpenBox.Persistence.Tests.Unit.Repositories;

/// <summary>
/// Test the Brand Repository logic.
/// </summary>
/// <remarks>The brand entity is used for this tests.</remarks>
public class BrandRepositoryTest : IClassFixture<BrandSeedDataFixture>
{
    private readonly IBrandRepository _brandRepository;
    private readonly OpenBoxDbContext _dbContext;

    public BrandRepositoryTest(BrandSeedDataFixture fixture)
    {
        _brandRepository = new BrandRepository(fixture.DbContext);
        _dbContext = fixture.DbContext;
    }

    [Fact]
    public async Task Get_A_Brand_By_Name()
    {
        // Arrange
        var brand = new BrandFaker().Generate();
        _dbContext.Add(brand);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _brandRepository.GetByNameAsync(brand.Name, false);

        // Assert
        Assert.NotNull(result);
    }
}