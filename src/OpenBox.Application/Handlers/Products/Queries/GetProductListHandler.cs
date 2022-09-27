using Ardalis.GuardClauses;
using OpenBox.Application.Common;
using OpenBox.Application.Repositories;
using OpenBox.Domain.Entities;

namespace OpenBox.Application.Handlers.Products.Queries;

public class GetProductListHandler : IQueryHandler<GetProductList, IEnumerable<ProductListItem>>
{
    private readonly IProductRepository _productRepository;

    public GetProductListHandler(IProductRepository productRepository)
    {
        _productRepository = Guard.Against.Null(productRepository, nameof(productRepository));
    }

    public async Task<IEnumerable<ProductListItem>> Handle(GetProductList query, CancellationToken ct)
    {
        Guard.Against.Null(query, nameof(query));
        
        IEnumerable<Product> products;
        
        if (query.PageIndex is not null && query.PageSize is not null)
        {
            products = await _productRepository.GetAllAsync(query.PageIndex.Value, query.PageSize.Value, ct);
        }
        else
        {
            products = await _productRepository.GetAllAsync(ct);
        }

        return products
            .Select(product =>
                new ProductListItem(product.Id, product.Name, product.Description, product.Price, product.Brand.Name))
            .ToList();
    }
}

public record GetProductList(
    int? PageIndex,
    int? PageSize
);

public record ProductListItem(
    Guid Id,
    string Name,
    string? Description,
    uint Price,
    string? Brand
);