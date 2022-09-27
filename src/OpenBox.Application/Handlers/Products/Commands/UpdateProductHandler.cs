using Ardalis.GuardClauses;
using OpenBox.Application.Common;
using OpenBox.Application.Exceptions;
using OpenBox.Application.Repositories;

namespace OpenBox.Application.Handlers.Products.Commands;

public class UpdateProductHandler : ICommandHandler<UpdateProduct>
{
    private readonly IProductRepository _productRepository;
    private readonly IBrandRepository _brandRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateProductHandler(IProductRepository productRepository, IBrandRepository brandRepository,
        IUnitOfWork unitOfWork)
    {
        _productRepository = Guard.Against.Null(productRepository, nameof(productRepository));
        _brandRepository = Guard.Against.Null(brandRepository, nameof(brandRepository));
        _unitOfWork = Guard.Against.Null(unitOfWork, nameof(unitOfWork));
    }

    public async Task Handle(UpdateProduct command, CancellationToken ct)
    {
        Guard.Against.Null(command, nameof(command));

        var product = await _productRepository.GetAsync(command.Id, true, ct);

        if (product is null)
        {
            throw new EntityNotFoundException("The product does not exist.");
        }
        
        var brand = await _brandRepository.GetByNameAsync(command.Brand, true);

        if (brand is null)
        {
            throw new InvalidOperationException("The brand does not exist.");
        }

        product.Name = command.Name;
        product.Description = command.Description;
        product.Price = command.Price;
        product.Brand = brand;

        _productRepository.Update(product);
        await _unitOfWork.SaveChangesAsync(ct);
    }
}

public record UpdateProduct(
    Guid Id,
    string Name,
    string? Description,
    uint Price,
    string Brand
);