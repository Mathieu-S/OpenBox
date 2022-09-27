using Ardalis.GuardClauses;
using Microsoft.EntityFrameworkCore;
using OpenBox.Application.Common;
using OpenBox.Domain.Common;

namespace OpenBox.Persistence.Common;

/// <inheritdoc/>
public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : Entity
{
    protected readonly OpenBoxDbContext DbContext;

    /// <summary>
    /// Ctor
    /// </summary>
    /// <param name="dbContext"></param>
    /// <exception cref="ArgumentNullException">Throw when the dbContext is null.</exception>
    protected RepositoryBase(OpenBoxDbContext dbContext)
    {
        DbContext = Guard.Against.Null(dbContext, nameof(dbContext));
    }

    /// <inheritdoc/>
    public virtual async Task<IEnumerable<T>> GetAllAsync(CancellationToken ct)
    {
        return await DbContext
            .Set<T>()
            .OrderBy(x => x.Id)
            .ToArrayAsync(ct);
    }

    /// <inheritdoc/>
    public virtual async Task<IEnumerable<T>> GetAllAsync(int pageIndex, int pageSize, CancellationToken ct)
    {
        return await DbContext
            .Set<T>()
            .OrderBy(x => x.Id)
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToArrayAsync(ct);
    }

    /// <inheritdoc/>
    public virtual async Task<T?> GetAsync(Guid id, bool asTracking, CancellationToken ct)
    {
        Guard.Against.NullOrEmpty(id, nameof(id), "The id of entity cannot be null or empty.");

        var query = DbContext
            .Set<T>()
            .AsQueryable();

        if (asTracking)
        {
            query = query.AsTracking();
        }

        return await query
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync(ct);
    }

    /// <inheritdoc/>
    public virtual Guid Add(T entity)
    {
        Guard.Against.Null(entity, nameof(entity), "The entity cannot be null.");

        DbContext
            .Set<T>()
            .Add(entity);

        return entity.Id;
    }

    /// <inheritdoc/>
    public virtual IEnumerable<Guid> Add(IEnumerable<T> entities)
    {
        Guard.Against.NullOrEmpty(entities, nameof(entities), "The entities cannot be null or empty.");

        DbContext
            .Set<T>()
            .AddRange(entities);

        return entities
            .Select(x => x.Id)
            .ToArray();
    }

    /// <inheritdoc/>
    public virtual void Update(T entity)
    {
        Guard.Against.Null(entity, nameof(entity), "The entity cannot be null.");

        DbContext
            .Set<T>()
            .Update(entity);
    }

    /// <inheritdoc/>
    public virtual void Update(IEnumerable<T> entities)
    {
        Guard.Against.NullOrEmpty(entities, nameof(entities), "The entities cannot be null or empty.");

        DbContext
            .Set<T>()
            .UpdateRange(entities);
    }

    /// <inheritdoc/>
    public virtual void Delete(T entity)
    {
        Guard.Against.Null(entity, nameof(entity), "The entity cannot be null.");

        DbContext
            .Set<T>()
            .Remove(entity);
    }

    /// <inheritdoc/>
    public virtual void Delete(IEnumerable<T> entities)
    {
        Guard.Against.NullOrEmpty(entities, nameof(entities), "The entities cannot be null or empty.");

        DbContext
            .Set<T>()
            .RemoveRange(entities);
    }
}