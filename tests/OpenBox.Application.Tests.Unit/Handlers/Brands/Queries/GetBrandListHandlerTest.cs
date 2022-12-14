using NSubstitute;
using OpenBox.Application.Handlers.Brands.Queries;
using OpenBox.Application.Repositories;
using OpenBox.Domain.Entities;
using OpenBox.Domain.Tests.Unit.Fakers;
using Xunit;

namespace OpenBox.Application.Tests.Unit.Handlers.Brands.Queries;

public class GetBrandListHandlerTest
{
    private readonly IBrandRepository _brandRepository;
    private readonly GetBrandListHandler _handler;

    public GetBrandListHandlerTest()
    {
        _brandRepository = Substitute.For<IBrandRepository>();
        _handler = new GetBrandListHandler(_brandRepository);
    }

    [Fact]
    public async Task Handle()
    {
        // Arrange
        var brand = new BrandFaker().Generate();
        _brandRepository
            .GetAllAsync(CancellationToken.None)
            .Returns(new List<Brand> { brand });

        // Act
        var result = (await _handler.Handle(new GetBrandList(null, null), CancellationToken.None)).ToList();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(brand.Id, result.First().Id);
        Assert.Equal(brand.Name, result.First().Name);
    }
}