using NSubstitute;
using OpenBox.Application.Handlers.Products.Queries;
using OpenBox.Application.Repositories;
using OpenBox.Domain.Entities;
using OpenBox.Domain.Tests.Unit.Fakers;
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
        var product = new ProductFaker().Generate();
        _productRepository
            .GetAllAsync(CancellationToken.None)
            .Returns(new List<Product> { product });

        // Act
        var result = (await _handler.Handle(query, CancellationToken.None)).ToList();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(product.Id, result.First().Id);
        Assert.Equal(product.Name, result.First().Name);
        Assert.Equal(product.Description, result.First().Description);
        Assert.Equal(product.Price, result.First().Price);
        Assert.Equal(product.Brand.Name, result.First().Brand);
    }
}