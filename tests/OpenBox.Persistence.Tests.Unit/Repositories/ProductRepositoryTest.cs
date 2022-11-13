using OpenBox.Application.Repositories;
using OpenBox.Domain.Tests.Unit.Fakers;
using OpenBox.Persistence.Repositories;
using OpenBox.Persistence.Tests.Unit.Fixtures;
using Xunit;

namespace OpenBox.Persistence.Tests.Unit.Repositories;

public class ProductRepositoryTest : IClassFixture<ProductSeedDataFixture>
{
    private readonly IProductRepository _productRepository;
    private readonly OpenBoxDbContext _dbContext;

    public ProductRepositoryTest(ProductSeedDataFixture fixture)
    {
        _productRepository = new ProductRepository(fixture.DbContext);
        _dbContext = fixture.DbContext;
    }

    [Fact]
    public async Task Get_All()
    {
        // Arrange
        var products = new ProductFaker().Generate(2);
        _dbContext.AddRange(products);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = (await _productRepository.GetAllAsync(CancellationToken.None)).ToList();

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.NotEqual(Guid.Empty, result.First().BrandId);
        Assert.NotNull(result.First().Brand);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task Get_One(bool asTracking)
    {
        // Arrange
        var product = new ProductFaker().Generate();
        _dbContext.Add(product);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _productRepository.GetAsync(product.Id, asTracking, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.NotEqual(Guid.Empty, result.BrandId);
        Assert.NotNull(result.Brand);
    }

    [Fact]
    public async Task Get_Unknown()
    {
        // Act
        var result = await _productRepository.GetAsync(Guid.NewGuid(), false, CancellationToken.None);

        // Assert
        Assert.Null(result);
    }
}