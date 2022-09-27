using Ardalis.GuardClauses;
using Microsoft.EntityFrameworkCore;
using OpenBox.Application.Repositories;
using OpenBox.Domain.Entities;
using OpenBox.Persistence.Common;

namespace OpenBox.Persistence.Repositories;

/// <inheritdoc cref="IProductRepository" />
public class ProductRepository : RepositoryBase<Product>, IProductRepository
{
    public ProductRepository(OpenBoxDbContext dbContext) : base(dbContext)
    {
    }

    public override async Task<IEnumerable<Product>> GetAllAsync(CancellationToken ct)
    {
        return await DbContext
            .Products
            .Include(x => x.Brand)
            .OrderBy(x => x.Id)
            .ToArrayAsync(ct);
    }

    public override async Task<IEnumerable<Product>> GetAllAsync(int pageIndex, int pageSize, CancellationToken ct)
    {
        return await DbContext
            .Products
            .Include(x => x.Brand)
            .OrderBy(x => x.Id)
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToArrayAsync(ct);
    }

    public override async Task<Product?> GetAsync(Guid id, bool asTracking, CancellationToken ct)
    {
        Guard.Against.NullOrEmpty(id, nameof(id), "The id of entity cannot be null or empty.");

        var query = DbContext
            .Products
            .Include(x => x.Brand)
            .AsQueryable();

        if (asTracking)
        {
            query = query.AsTracking();
        }

        return await query
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync(ct);
    }
}