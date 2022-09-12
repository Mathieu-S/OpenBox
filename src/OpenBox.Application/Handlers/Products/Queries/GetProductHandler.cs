using Ardalis.GuardClauses;
using OpenBox.Application.Common;
using OpenBox.Application.Exceptions;
using OpenBox.Application.Repositories;

namespace OpenBox.Application.Handlers.Products.Queries;

public class GetProductHandler : IQueryHandler<GetProduct, ProductItem>
{
    private readonly IProductRepository _productRepository;

    public GetProductHandler(IProductRepository productRepository)
    {
        _productRepository = Guard.Against.Null(productRepository, nameof(productRepository));
    }

    public async Task<ProductItem> Handle(GetProduct query, CancellationToken ct)
    {
        Guard.Against.Null(query, nameof(query));

        var product = await _productRepository.GetAsync(query.Id);

        if (product is null)
        {
            throw new EntityNotFoundException();
        }

        return new ProductItem(product.Id, product.Name, product.Description, product.Price, product.Brand.Name);
    }
}

public record GetProduct(
    Guid Id
);

public record ProductItem(
    Guid Id,
    string Name,
    string? Description,
    uint Price,
    string Brand
);