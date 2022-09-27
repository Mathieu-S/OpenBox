using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using OpenBox.Application.Common;
using OpenBox.Application.Exceptions;
using OpenBox.Application.Handlers.Brands.Commands;
using OpenBox.Application.Handlers.Brands.Queries;
using OpenBox.WebApi.Controllers;
using Xunit;

namespace OpenBox.WebApi.Tests.Unit.Controllers;

public class BrandsControllerTest
{
    private readonly BrandsController _controller;

    public BrandsControllerTest()
    {
        var logger = new NullLogger<BrandsController>();
        _controller = new BrandsController(logger);
    }

    [Fact]
    public async Task Get_All()
    {
        // Arrange
        var handler = Substitute.For<IQueryHandler<GetBrandList, IEnumerable<BrandListItem>>>();
        handler
            .Handle(Arg.Any<GetBrandList>(), Arg.Any<CancellationToken>())
            .Returns(new List<BrandListItem>());

        // Act
        var response = await _controller.Get(handler, new GetBrandList(null, null), CancellationToken.None);

        // Assert
        Assert.NotNull(response);
        var content = Assert.IsAssignableFrom<OkObjectResult>(response);
        Assert.NotNull(content.Value);
        Assert.IsAssignableFrom<IEnumerable<BrandListItem>>(content.Value);
    }

    [Fact]
    public async Task Get_One()
    {
        // Arrange
        var id = Guid.NewGuid();
        var handler = Substitute.For<IQueryHandler<GetBrand, BrandItem>>();
        handler
            .Handle(Arg.Any<GetBrand>(), Arg.Any<CancellationToken>())
            .Returns(new BrandItem(id, "Acme"));

        // Act
        var response = await _controller.Get(handler, id, CancellationToken.None);

        // Assert
        Assert.NotNull(response);
        var content = Assert.IsAssignableFrom<OkObjectResult>(response);
        Assert.NotNull(content.Value);
        var brandDto = Assert.IsAssignableFrom<BrandItem>(content.Value);
        Assert.Equal(id, brandDto.Id);
    }

    [Fact]
    public async Task Get_One_NotFount()
    {
        // Arrange
        var handler = Substitute.For<IQueryHandler<GetBrand, BrandItem>>();
        handler
            .Handle(Arg.Any<GetBrand>(), Arg.Any<CancellationToken>())
            .ThrowsAsync(new EntityNotFoundException());

        // Act
        var response = await _controller.Get(handler, Guid.NewGuid(), CancellationToken.None);

        // Assert
        Assert.NotNull(response);
        Assert.IsAssignableFrom<NotFoundResult>(response);
    }

    [Fact]
    public async Task Get_One_BadRequest()
    {
        // Arrange
        var handler = Substitute.For<IQueryHandler<GetBrand, BrandItem>>();
        handler
            .Handle(Arg.Any<GetBrand>(), Arg.Any<CancellationToken>())
            .ThrowsAsync(new ArgumentException());

        // Act
        var response = await _controller.Get(handler, Guid.NewGuid(), CancellationToken.None);

        // Assert
        Assert.NotNull(response);
        Assert.IsAssignableFrom<BadRequestObjectResult>(response);
    }

    [Fact]
    public async Task Post()
    {
        // Arrange
        var brandId = Guid.NewGuid();
        var handler = Substitute.For<ICommandHandler<CreateBrand, Guid>>();
        handler
            .Handle(Arg.Any<CreateBrand>(), Arg.Any<CancellationToken>())
            .Returns(brandId);

        // Act
        var response = await _controller.Post(handler, new CreateBrand("Acme"), CancellationToken.None);

        // Assert
        Assert.NotNull(response);
        var content = Assert.IsAssignableFrom<CreatedAtActionResult>(response);
        Assert.NotNull(content.Value);
        Assert.Equal("Get", content.ActionName);
        Assert.Equal(brandId.ToString(), content.Value);
    }

    [Fact]
    public async Task Post_BadRequest()
    {
        // Arrange
        var handler = Substitute.For<ICommandHandler<CreateBrand, Guid>>();
        handler
            .Handle(Arg.Any<CreateBrand>(), Arg.Any<CancellationToken>())
            .ThrowsAsync(new ArgumentException());

        // Act
        var response = await _controller.Post(handler, new CreateBrand("Acme"), CancellationToken.None);

        // Assert
        Assert.NotNull(response);
        Assert.IsAssignableFrom<BadRequestObjectResult>(response);
    }

    [Fact]
    public async Task Put()
    {
        // Arrange
        var brandToUpdate = new UpdateBrand(Guid.NewGuid(), "Acme");
        var handler = Substitute.For<ICommandHandler<UpdateBrand>>();

        // Act
        var response = await _controller.Put(handler, brandToUpdate.Id, brandToUpdate, CancellationToken.None);

        // Assert
        Assert.NotNull(response);
        var content = Assert.IsAssignableFrom<OkObjectResult>(response);
        Assert.NotNull(content.Value);
        var brandDto = Assert.IsAssignableFrom<UpdateBrand>(content.Value);
        Assert.Equal(brandToUpdate.Id, brandDto.Id);
        Assert.Equal(brandToUpdate.Name, brandDto.Name);
        await handler
            .Received()
            .Handle(Arg.Any<UpdateBrand>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Put_NotFound()
    {
        // Arrange
        var id = Guid.NewGuid();
        var handler = Substitute.For<ICommandHandler<UpdateBrand>>();
        handler
            .Handle(Arg.Any<UpdateBrand>(), Arg.Any<CancellationToken>())
            .ThrowsAsync(new EntityNotFoundException());

        // Act
        var response = await _controller.Put(handler, id, new UpdateBrand(id, "Acme"), CancellationToken.None);

        // Assert
        Assert.NotNull(response);
        Assert.IsAssignableFrom<NotFoundResult>(response);
    }

    [Fact]
    public async Task Put_BadRequest()
    {
        // Arrange
        var id = Guid.NewGuid();
        var handler = Substitute.For<ICommandHandler<UpdateBrand>>();
        handler
            .Handle(Arg.Any<UpdateBrand>(), Arg.Any<CancellationToken>())
            .ThrowsAsync(new ArgumentException());

        // Act
        var response = await _controller.Put(handler, id, new UpdateBrand(id, "Acme"), CancellationToken.None);

        // Assert
        Assert.NotNull(response);
        Assert.IsAssignableFrom<BadRequestObjectResult>(response);
    }

    [Fact]
    public async Task Delete()
    {
        // Arrange
        var handler = Substitute.For<ICommandHandler<DeleteBrand>>();

        // Act
        var response = await _controller.Delete(handler, Guid.NewGuid(), CancellationToken.None);

        // Assert
        Assert.NotNull(response);
        Assert.IsAssignableFrom<NoContentResult>(response);
        await handler
            .Received()
            .Handle(Arg.Any<DeleteBrand>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Delete_NotFound()
    {
        // Arrange
        var handler = Substitute.For<ICommandHandler<DeleteBrand>>();
        handler
            .Handle(Arg.Any<DeleteBrand>(), Arg.Any<CancellationToken>())
            .ThrowsAsync(new EntityNotFoundException());

        // Act
        var response = await _controller.Delete(handler, Guid.NewGuid(), CancellationToken.None);

        // Assert
        Assert.NotNull(response);
        Assert.IsAssignableFrom<NotFoundResult>(response);
    }

    [Fact]
    public async Task Delete_BadRequest()
    {
        // Arrange
        var handler = Substitute.For<ICommandHandler<DeleteBrand>>();
        handler
            .Handle(Arg.Any<DeleteBrand>(), Arg.Any<CancellationToken>())
            .ThrowsAsync(new ArgumentException());

        // Act
        var response = await _controller.Delete(handler, Guid.NewGuid(), CancellationToken.None);

        // Assert
        Assert.NotNull(response);
        Assert.IsAssignableFrom<BadRequestObjectResult>(response);
    }
}