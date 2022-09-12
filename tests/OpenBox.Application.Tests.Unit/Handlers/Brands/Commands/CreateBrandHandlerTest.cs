using NSubstitute;
using OpenBox.Application.Common;
using OpenBox.Application.Handlers.Brands.Commands;
using OpenBox.Application.Repositories;
using OpenBox.Domain.Entities;
using Xunit;

namespace OpenBox.Application.Tests.Unit.Handlers.Brands.Commands;

public class CreateBrandHandlerTest
{
    private readonly IBrandRepository _brandRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly CreateBrandHandler _handler;

    public CreateBrandHandlerTest()
    {
        _brandRepository = Substitute.For<IBrandRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _handler = new CreateBrandHandler(_brandRepository, _unitOfWork);
    }

    [Fact]
    public async Task Handle()
    {
        // Arrange
        var command = new CreateBrand("Acme");
        var createdId = Guid.NewGuid();
        _brandRepository
            .Add(Arg.Any<Brand>())
            .Returns(createdId);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(createdId, result);
        await _unitOfWork
            .Received()
            .SaveChangesAsync();
    }

    [Theory]
    [MemberData(nameof(InvalidCommands))]
    public async Task Handle_Throw_ArgumentException(CreateBrand command)
    {
        // Act & Assert
        await Assert.ThrowsAnyAsync<ArgumentException>(() => _handler.Handle(command, CancellationToken.None));
    }

    public static IEnumerable<object?[]> InvalidCommands()
    {
        yield return new object?[]
        {
            null
        };
        yield return new object?[]
        {
            new CreateBrand(null!)
        };
        yield return new object?[]
        {
            new CreateBrand(string.Empty)
        };
    }
}