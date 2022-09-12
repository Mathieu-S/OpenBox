using OpenBox.Application.Repositories;
using OpenBox.Domain.Entities;
using OpenBox.Persistence.Repositories;
using OpenBox.Persistence.Tests.Unit.Fixtures;
using Xunit;

namespace OpenBox.Persistence.Tests.Unit.Repositories;

public class ProductRepositoryTest : IClassFixture<ProductSeedDataFixture>
{
    private readonly IProductRepository _productRepository;
    private readonly IEnumerable<Product> _productsInDb;

    public ProductRepositoryTest(ProductSeedDataFixture fixture)
    {
        _productRepository = new ProductRepository(fixture.DbContext);
        _productsInDb = fixture.DbContext.Products;
    }

    [Fact]
    public async Task Get_All()
    {
        // Act
        var result = await _productRepository.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.NotNull(result.First().Brand);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task Get_One(bool asTracking)
    {
        // Arrange
        var brandId = _productsInDb.First().Id;

        // Act
        var result = await _productRepository.GetAsync(brandId, asTracking);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.Brand);
    }

    [Fact]
    public async Task Get_Unknown()
    {
        // Act
        var result = await _productRepository.GetAsync(Guid.NewGuid());

        // Assert
        Assert.Null(result);
    }
}