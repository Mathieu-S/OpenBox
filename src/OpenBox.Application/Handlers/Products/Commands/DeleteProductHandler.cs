using Ardalis.GuardClauses;
using OpenBox.Application.Common;
using OpenBox.Application.Exceptions;
using OpenBox.Application.Repositories;

namespace OpenBox.Application.Handlers.Products.Commands;

public class DeleteProductHandler : ICommandHandler<DeleteProduct>
{
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteProductHandler(IProductRepository productRepository, IUnitOfWork unitOfWork)
    {
        _productRepository = Guard.Against.Null(productRepository, nameof(productRepository));
        _unitOfWork = Guard.Against.Null(unitOfWork, nameof(unitOfWork));
    }

    public async Task Handle(DeleteProduct command, CancellationToken ct)
    {
        Guard.Against.Null(command, nameof(command));

        var product = await _productRepository.GetAsync(command.Id, true, ct);

        if (product is null)
        {
            throw new EntityNotFoundException();
        }
        
        _productRepository.Delete(product);
        await _unitOfWork.SaveChangesAsync(ct);
    }
}

public record DeleteProduct(
    Guid Id
);