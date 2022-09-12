using OpenBox.Application.Repositories;
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

    public BrandRepositoryTest(BrandSeedDataFixture fixture)
    {
        _brandRepository = new BrandRepository(fixture.DbContext);
    }

    [Fact]
    public async Task Get_A_Brand_By_Name()
    {
        // Act
        var result = await _brandRepository.GetByNameAsync("Acme");

        // Assert
        Assert.NotNull(result);
    }
}