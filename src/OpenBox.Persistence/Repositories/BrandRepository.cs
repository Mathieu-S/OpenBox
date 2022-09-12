using Ardalis.GuardClauses;
using Microsoft.EntityFrameworkCore;
using OpenBox.Application.Repositories;
using OpenBox.Domain.Entities;
using OpenBox.Persistence.Common;

namespace OpenBox.Persistence.Repositories;

/// <inheritdoc cref="IBrandRepository" />
public class BrandRepository : RepositoryBase<Brand>, IBrandRepository
{
    public BrandRepository(OpenBoxDbContext dbContext) : base(dbContext)
    {
    }

    /// <inheritdoc/>
    public async Task<Brand?> GetByNameAsync(string name)
    {
        Guard.Against.NullOrWhiteSpace(name);

        return await DbContext.Brands.Where(x => x.Name == name).FirstOrDefaultAsync();
    }

    /// <inheritdoc/>
    public async Task<Brand?> GetByNameAsync(string name, bool asTracking)
    {
        Guard.Against.NullOrWhiteSpace(name);
        
        var query = DbContext
            .Brands
            .AsQueryable();

        if (asTracking)
        {
            query = query.AsTracking();
        }

        return await query
            .Where(x => x.Name == name)
            .FirstOrDefaultAsync();
    }
}