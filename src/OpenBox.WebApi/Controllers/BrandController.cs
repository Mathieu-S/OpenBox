using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenBox.Application.Common;
using OpenBox.Application.Exceptions;
using OpenBox.Application.Handlers.Brands.Commands;
using OpenBox.Application.Handlers.Brands.Queries;

namespace OpenBox.WebApi.Controllers;

/// <summary>
/// Controller API to manage brands.
/// </summary>
[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public class BrandController : ControllerBase
{
    private readonly ILogger<BrandController> _logger;

    public BrandController(ILogger<BrandController> logger)
    {
        _logger = Guard.Against.Null(logger, nameof(logger));
    }

    /// <summary>
    /// Get all brands.
    /// </summary>
    /// <param name="handler">The handler bound to the controller route.</param>
    /// <param name="query"></param>
    /// <param name="ct">The CancellationToken.</param>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<BrandListItem>))]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Get(
        [FromServices] IQueryHandler<GetBrandList, IEnumerable<BrandListItem>> handler,
        [FromQuery] GetBrandList query,
        CancellationToken ct
    )
    {
        var brands = await handler.Handle(query, ct);
        return Ok(brands);
    }

    /// <summary>
    /// Get a brand by ID.
    /// </summary>
    /// <param name="handler">The handler bound to the controller route.</param>
    /// <param name="id">The Id of Brand.</param>
    /// <param name="ct">The CancellationToken.</param>
    /// <returns></returns>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BrandItem))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Get(
        [FromServices] IQueryHandler<GetBrand, BrandItem> handler,
        [FromRoute] Guid id,
        CancellationToken ct
    )
    {
        BrandItem brand;

        try
        {
            brand = await handler.Handle(new GetBrand(id), ct);
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

        return Ok(brand);
    }

    /// <summary>
    /// Create a brand.
    /// </summary>
    /// <param name="handler">The handler bound to the controller route.</param>
    /// <param name="brandDto">The dto.</param>
    /// <param name="ct">The CancellationToken.</param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Guid))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Post(
        [FromServices] ICommandHandler<CreateBrand, Guid> handler,
        [FromBody] CreateBrand brandDto,
        CancellationToken ct
    )
    {
        Guid idBrand;

        try
        {
            idBrand = await handler.Handle(brandDto, ct);
        }
        catch (ArgumentException e)
        {
            return BadRequest(e.Message);
        }

        _logger.LogInformation("The brand '{brandDto}' has been created with ID:{idBrand}.", brandDto.Name,
            idBrand.ToString());
        return CreatedAtAction("Get", idBrand.ToString());
    }

    /// <summary>
    /// Update a brand.
    /// </summary>
    /// <param name="handler">The handler bound to the controller route.</param>
    /// <param name="id">The Id of Brand.</param>
    /// <param name="brandDto">The dto.</param>
    /// <param name="ct">The CancellationToken.</param>
    /// <returns></returns>
    [Authorize]
    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BrandItem))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Put(
        [FromServices] ICommandHandler<UpdateBrand> handler,
        [FromRoute] Guid id,
        [FromBody] UpdateBrand brandDto,
        CancellationToken ct)
    {
        if (id != brandDto.Id) return BadRequest("The Id in the route is not equal to the Id in body");

        try
        {
            await handler.Handle(brandDto, ct);
        }
        catch (EntityNotFoundException)
        {
            return NotFound();
        }
        catch (ArgumentException e)
        {
            return BadRequest(e.Message);
        }

        _logger.LogInformation("The brand with ID:'{updatedBrand}' has been updated.", brandDto.Id.ToString());
        return Ok(brandDto);
    }

    /// <summary>
    /// Delete a brand.
    /// </summary>
    /// <param name="handler">The handler bound to the controller route.</param>
    /// <param name="id">The Id of Brand.</param>
    /// <param name="ct">The CancellationToken.</param>
    /// <returns></returns>
    [Authorize]
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Delete(
        [FromServices] ICommandHandler<DeleteBrand> handler,
        [FromRoute] Guid id,
        CancellationToken ct
    )
    {
        try
        {
            await handler.Handle(new DeleteBrand(id), ct);
        }
        catch (EntityNotFoundException)
        {
            return NotFound();
        }
        catch (ArgumentException e)
        {
            return BadRequest(e.Message);
        }

        _logger.LogInformation("The brand '{id}' has been removed.", id.ToString());
        return NoContent();
    }
}