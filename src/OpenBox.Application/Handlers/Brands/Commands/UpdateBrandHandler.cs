using Ardalis.GuardClauses;
using OpenBox.Application.Common;
using OpenBox.Application.Exceptions;
using OpenBox.Application.Repositories;

namespace OpenBox.Application.Handlers.Brands.Commands;

public class UpdateBrandHandler : ICommandHandler<UpdateBrand>
{
    private readonly IBrandRepository _brandRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateBrandHandler(IBrandRepository brandRepository, IUnitOfWork unitOfWork)
    {
        _brandRepository = Guard.Against.Null(brandRepository, nameof(brandRepository));
        _unitOfWork = Guard.Against.Null(unitOfWork, nameof(unitOfWork));
    }

    public async Task Handle(UpdateBrand command, CancellationToken ct)
    {
        Guard.Against.Null(command, nameof(command));

        var brand = await _brandRepository.GetAsync(command.Id, true, ct);
        
        if (brand is null)
        {
            throw new EntityNotFoundException();
        }
        
        brand.Name = command.Name;
        
        _brandRepository.Update(brand);
        await _unitOfWork.SaveChangesAsync(ct);
    }
}

public record UpdateBrand(
    Guid Id,
    string Name
);