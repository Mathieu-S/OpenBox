using NSubstitute;
using NSubstitute.ReturnsExtensions;
using OpenBox.Application.Exceptions;
using OpenBox.Application.Handlers.Brands.Queries;
using OpenBox.Application.Repositories;
using OpenBox.Domain.Tests.Unit.Fakers;
using Xunit;

namespace OpenBox.Application.Tests.Unit.Handlers.Brands.Queries;

public class GetBrandHandlerTest
{
    private readonly IBrandRepository _brandRepository;
    private readonly GetBrandHandler _handler;

    public GetBrandHandlerTest()
    {
        _brandRepository = Substitute.For<IBrandRepository>();
        _handler = new GetBrandHandler(_brandRepository);
    }

    [Fact]
    public async Task Handle_Valid()
    {
        // Arrange
        var brand = new BrandFaker().Generate();
        var query = new GetBrand(brand.Id);
        _brandRepository
            .GetAsync(Arg.Any<Guid>(), false, CancellationToken.None)
            .Returns(brand);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(brand.Id, result.Id);
        Assert.Equal(brand.Name, result.Name);
    }

    [Fact]
    public async Task Handle_Throw_EntityNotFoundException()
    {
        // Arrange
        var query = new GetBrand(Guid.NewGuid());
        _brandRepository
            .GetAsync(Arg.Any<Guid>(), false, CancellationToken.None)
            .ReturnsNull();

        // Act & Assert
        await Assert.ThrowsAsync<EntityNotFoundException>(() => _handler.Handle(query, CancellationToken.None));
    }

    [Theory]
    [MemberData(nameof(InvalidQueries))]
    public async Task Handle_Throw_ArgumentException(GetBrand query)
    {
        // Act & Assert
        await Assert.ThrowsAnyAsync<ArgumentException>(() => _handler.Handle(query, CancellationToken.None));
    }

    public static IEnumerable<object?[]> InvalidQueries()
    {
        yield return new object?[]
        {
            null
        };
        yield return new object?[]
        {
            new GetBrand(Guid.Empty)
        };
    }
}