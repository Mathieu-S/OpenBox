using Ardalis.GuardClauses;
using OpenBox.Application.Common;
using OpenBox.Application.Repositories;
using OpenBox.Domain.Entities;

namespace OpenBox.Application.Handlers.Products.Commands;

public class CreateProductHandler : ICommandHandler<CreateProduct, Guid>
{
    private readonly IProductRepository _productRepository;
    private readonly IBrandRepository _brandRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateProductHandler(IProductRepository productRepository, IBrandRepository brandRepository,
        IUnitOfWork unitOfWork)
    {
        _productRepository = Guard.Against.Null(productRepository, nameof(productRepository));
        _brandRepository = Guard.Against.Null(brandRepository, nameof(brandRepository));
        _unitOfWork = Guard.Against.Null(unitOfWork, nameof(unitOfWork));
    }

    public async Task<Guid> Handle(CreateProduct command, CancellationToken ct)
    {
        Guard.Against.Null(command, nameof(command));

        var brand = await _brandRepository.GetByNameAsync(command.Brand, true);

        if (brand is null)
        {
            throw new InvalidOperationException("The brand does not exist.");
        }

        var product = new Product
        {
            Name = command.Name,
            Description = command.Description,
            Price = command.Price,
            Brand = brand
        };

        var id = _productRepository.Add(product);
        await _unitOfWork.SaveChangesAsync(ct);
        return id;
    }
}

public record CreateProduct(
    string Name,
    string? Description,
    uint Price,
    string Brand
);