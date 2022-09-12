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
}