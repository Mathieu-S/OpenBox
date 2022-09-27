using NSubstitute;
using NSubstitute.ReturnsExtensions;
using OpenBox.Application.Common;
using OpenBox.Application.Exceptions;
using OpenBox.Application.Handlers.Brands.Commands;
using OpenBox.Application.Repositories;
using OpenBox.Domain.Entities;
using Xunit;

namespace OpenBox.Application.Tests.Unit.Handlers.Brands.Commands;

public class DeleteBrandHandlerTest
{
    private readonly IBrandRepository _brandRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly DeleteBrandHandler _handler;

    public DeleteBrandHandlerTest()
    {
        _brandRepository = Substitute.For<IBrandRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _handler = new DeleteBrandHandler(_brandRepository, _unitOfWork);
    }

    [Fact]
    public async Task Handle()
    {
        // Arrange
        var command = new DeleteBrand(Guid.NewGuid());
        _brandRepository
            .GetAsync(Arg.Any<Guid>(), Arg.Any<bool>(), CancellationToken.None)
            .Returns(new Brand());

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _brandRepository
            .Received()
            .Delete(Arg.Any<Brand>());
        await _unitOfWork
            .Received()
            .SaveChangesAsync(CancellationToken.None);
    }

    [Fact]
    public async Task Handle_Throw_EntityNotFoundException()
    {
        // Arrange
        var command = new DeleteBrand(Guid.NewGuid());
        _brandRepository
            .GetAsync(Arg.Any<Guid>(), Arg.Any<bool>(), CancellationToken.None)
            .ReturnsNull();

        // Act & Assert
        await Assert.ThrowsAsync<EntityNotFoundException>(() => _handler.Handle(command, CancellationToken.None));
    }

    [Theory]
    [MemberData(nameof(InvalidCommands))]
    public async Task Handle_Throw_ArgumentException(DeleteBrand command)
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
            new DeleteBrand(Guid.Empty)
        };
    }
}