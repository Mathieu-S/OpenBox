using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenBox.Application.Common;
using OpenBox.Application.Exceptions;
using OpenBox.Application.Handlers.Products.Commands;
using OpenBox.Application.Handlers.Products.Queries;

namespace OpenBox.WebApi.Controllers;

[ApiController]
[Authorize("Manager")]
[Route("[controller]")]
[Produces("application/json")]
public sealed class ProductsController : ControllerBase
{
    private readonly ILogger<ProductsController> _logger;

    public ProductsController(ILogger<ProductsController> logger)
    {
        _logger = Guard.Against.Null(logger, nameof(logger));
    }
    
    /// <summary>
    /// Get all products.
    /// </summary>
    /// <param name="handler">The handler bound to the controller route.</param>
    /// <param name="query"></param>
    /// <param name="ct">The CancellationToken.</param>
    /// <returns></returns>
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ProductListItem>))]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Get(
        [FromServices] IQueryHandler<GetProductList, IEnumerable<ProductListItem>> handler,
        [FromQuery] GetProductList query,
        CancellationToken ct
    )
    {
        var products = await handler.Handle(query, ct);
        return Ok(products);
    }
    
    /// <summary>
    /// Get a product by ID.
    /// </summary>
    /// <param name="handler">The handler bound to the controller route.</param>
    /// <param name="id">The Id of Product.</param>
    /// <param name="ct">The CancellationToken.</param>
    /// <returns></returns>
    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductItem))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Get(
        [FromServices] IQueryHandler<GetProduct, ProductItem> handler,
        [FromRoute] Guid id,
        CancellationToken ct
    )
    {
        ProductItem product;

        try
        {
            product = await handler.Handle(new GetProduct(id), ct);
        }
        catch (ArgumentException e)
        {
            return BadRequest(e.Message);
        }
        catch (EntityNotFoundException e)
        {
            _logger.LogTrace(e, e.Message);
            return NotFound();
        }

        return Ok(product);
    }
    
    /// <summary>
    /// Create a product.
    /// </summary>
    /// <param name="handler">The handler bound to the controller route.</param>
    /// <param name="productDto">The dto.</param>
    /// <param name="ct">The CancellationToken.</param>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Guid))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Post(
        [FromServices] ICommandHandler<CreateProduct, Guid> handler,
        [FromBody] CreateProduct productDto,
        CancellationToken ct
    )
    {
        Guid idProduct;

        try
        {
            idProduct = await handler.Handle(productDto, ct);
        }
        catch (ArgumentException e)
        {
            return BadRequest(e.Message);
        }
        catch (InvalidOperationException e)
        {
            return BadRequest(e.Message);
        }
        catch (ContextCannotBeSavedException e)
        {
            return Conflict(e.Message);
        }

        _logger.LogInformation("The brand '{brandDto}' has been created with ID:{idBrand}.", productDto.Name,
            idProduct.ToString());
        return CreatedAtAction("Get", idProduct.ToString());
    }
    
    /// <summary>
    /// Update a product.
    /// </summary>
    /// <param name="handler">The handler bound to the controller route.</param>
    /// <param name="id">The Id of Brand.</param>
    /// <param name="productDto">The dto.</param>
    /// <param name="ct">The CancellationToken.</param>
    /// <returns></returns>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductItem))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Put(
        [FromServices] ICommandHandler<UpdateProduct> handler,
        [FromRoute] Guid id,
        [FromBody] UpdateProduct productDto,
        CancellationToken ct)
    {
        if (id != productDto.Id) return BadRequest("The Id in the route is not equal to the Id in body");

        try
        {
            await handler.Handle(productDto, ct);
        }
        catch (EntityNotFoundException)
        {
            return NotFound();
        }
        catch (ArgumentException e)
        {
            return BadRequest(e.Message);
        }
        catch (InvalidOperationException e)
        {
            return BadRequest(e.Message);
        }
        catch (ContextCannotBeSavedException e)
        {
            return Conflict(e.Message);
        }

        _logger.LogInformation("The brand with ID:'{updatedBrand}' has been updated.", productDto.Id.ToString());
        return Ok(productDto);
    }
    
    /// <summary>
    /// Delete a brand.
    /// </summary>
    /// <param name="handler">The handler bound to the controller route.</param>
    /// <param name="id">The Id of Brand.</param>
    /// <param name="ct">The CancellationToken.</param>
    /// <returns></returns>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Delete(
        [FromServices] ICommandHandler<DeleteProduct> handler,
        [FromRoute] Guid id,
        CancellationToken ct
    )
    {
        try
        {
            await handler.Handle(new DeleteProduct(id), ct);
        }
        catch (EntityNotFoundException)
        {
            return NotFound();
        }
        catch (ArgumentException e)
        {
            return BadRequest(e.Message);
        }
        catch (ContextCannotBeSavedException e)
        {
            return Conflict(e.Message);
        }

        _logger.LogInformation("The brand '{id}' has been removed.", id.ToString());
        return NoContent();
    }
}