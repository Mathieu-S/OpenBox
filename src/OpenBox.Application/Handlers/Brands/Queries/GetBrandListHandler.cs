using Ardalis.GuardClauses;
using OpenBox.Application.Common;
using OpenBox.Application.Repositories;
using OpenBox.Domain.Entities;

namespace OpenBox.Application.Handlers.Brands.Queries;

public class GetBrandListHandler : IQueryHandler<GetBrandList, IEnumerable<BrandListItem>>
{
    private readonly IBrandRepository _brandRepository;

    public GetBrandListHandler(IBrandRepository brandRepository)
    {
        _brandRepository = Guard.Against.Null(brandRepository, nameof(brandRepository));
    }

    public async Task<IEnumerable<BrandListItem>> Handle(GetBrandList query, CancellationToken ct)
    {
        Guard.Against.Null(query, nameof(query));
        
        IEnumerable<Brand> brands;

        if (query.PageIndex is not null && query.PageSize is not null)
        {
            brands = await _brandRepository.GetAllAsync(query.PageIndex.Value, query.PageSize.Value);
        }
        else
        {
            brands = await _brandRepository.GetAllAsync();
        }

        return brands.Select(brand => new BrandListItem(brand.Id, brand.Name)).ToList();
    }
}

public record GetBrandList(
    int? PageIndex,
    int? PageSize
);

public record BrandListItem(
    Guid Id,
    string Name
);