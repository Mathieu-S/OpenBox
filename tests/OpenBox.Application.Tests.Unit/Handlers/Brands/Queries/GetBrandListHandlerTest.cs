using NSubstitute;
using OpenBox.Application.Handlers.Brands.Queries;
using OpenBox.Application.Repositories;
using OpenBox.Domain.Entities;
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
        var brand = new Brand { Id = Guid.NewGuid(), Name = "Acme" };
        _brandRepository
            .GetAllAsync()
            .Returns(new List<Brand> { brand });
        
        // Act
        var result = await _handler.Handle(new GetBrandList(null, null), CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(brand.Id, result.FirstOrDefault()!.Id);
        Assert.Equal(brand.Name, result.FirstOrDefault()!.Name);
    }
}