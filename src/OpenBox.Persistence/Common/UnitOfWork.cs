using Ardalis.GuardClauses;
using OpenBox.Application.Common;

namespace OpenBox.Persistence.Common;

public class UnitOfWork : IUnitOfWork
{
    private readonly OpenBoxDbContext _dbContext;

    public UnitOfWork(OpenBoxDbContext dbContext)
    {
        _dbContext = Guard.Against.Null(dbContext, nameof(dbContext));
    }

    public async Task SaveChangesAsync()
    {
        await _dbContext.SaveChangesAsync();
    }
}