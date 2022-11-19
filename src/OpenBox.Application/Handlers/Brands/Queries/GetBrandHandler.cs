using Ardalis.GuardClauses;
using OpenBox.Application.Common;
using OpenBox.Application.Exceptions;
using OpenBox.Application.Repositories;

namespace OpenBox.Application.Handlers.Brands.Queries;

public class GetBrandHandler : IQueryHandler<GetBrand, BrandItem>
{
    private readonly IBrandRepository _brandRepository;

    public GetBrandHandler(IBrandRepository brandRepository)
    {
        _brandRepository = Guard.Against.Null(brandRepository, nameof(brandRepository));
    }

    public async Task<BrandItem> Handle(GetBrand query, CancellationToken ct)
    {
        Guard.Against.Null(query);

        var brand = await _brandRepository.GetAsync(query.Id, false, ct);

        if (brand is null)
        {
            throw new EntityNotFoundException();
        }

        return new BrandItem(brand.Id, brand.Name);
    }
}

public record GetBrand(
    Guid Id
);

public record BrandItem(
    Guid Id,
    string Name
);