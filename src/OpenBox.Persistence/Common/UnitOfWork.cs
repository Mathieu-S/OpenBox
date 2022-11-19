using Ardalis.GuardClauses;
using OpenBox.Application.Common;
using OpenBox.Application.Exceptions;

namespace OpenBox.Persistence.Common;

/// <inheritdoc />
public class UnitOfWork : IUnitOfWork
{
    private readonly OpenBoxDbContext _dbContext;

    public UnitOfWork(OpenBoxDbContext dbContext)
    {
        _dbContext = Guard.Against.Null(dbContext, nameof(dbContext));
    }

    /// <inheritdoc />
    public async Task SaveChangesAsync()
    {
        try
        {
            await SaveChangesAsync(CancellationToken.None);
        }
        catch (Exception e)
        {
            throw new ContextCannotBeSavedException("An error occurred while saving data. Please check the logs.", e);
        }
    }

    /// <inheritdoc />
    public async Task SaveChangesAsync(CancellationToken ct)
    {
        try
        {
            await _dbContext.SaveChangesAsync(ct);
        }
        catch (Exception e)
        {
            throw new ContextCannotBeSavedException("An error occurred while saving data. Please check the logs.", e);
        }
    }
}