using NSubstitute;
using OpenBox.Application.Handlers.Products.Queries;
using OpenBox.Application.Repositories;
using OpenBox.Domain.Entities;
using Xunit;

namespace OpenBox.Application.Tests.Unit.Handlers.Products.Queries;

public class GetProductListHandlerTest
{
    private readonly IProductRepository _productRepository;
    private readonly GetProductListHandler _handler;

    public GetProductListHandlerTest()
    {
        _productRepository = Substitute.For<IProductRepository>();
        _handler = new GetProductListHandler(_productRepository);
    }

    [Fact]
    public async Task Handle_With_Brand()
    {
        // Arrange
        var query = new GetProductList(null, null);
        var brand = new Brand { Id = Guid.NewGuid(), Name = "Acme" };
        var product = new Product { Id = Guid.NewGuid(), Name = "Fork", Description = null, Price = 10, Brand = brand };
        _productRepository
            .GetAllAsync()
            .Returns(new List<Product> { product });

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(product.Id, result.FirstOrDefault()!.Id);
        Assert.Equal(product.Name, result.FirstOrDefault()!.Name);
        Assert.Null(result.FirstOrDefault()!.Description);
        Assert.Equal(product.Price, result.FirstOrDefault()!.Price);
        Assert.Equal(brand.Name, result.FirstOrDefault()!.Brand);
    }
}