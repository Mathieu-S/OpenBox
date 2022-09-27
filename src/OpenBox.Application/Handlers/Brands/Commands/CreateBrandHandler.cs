using Ardalis.GuardClauses;
using OpenBox.Application.Common;
using OpenBox.Application.Repositories;
using OpenBox.Domain.Entities;

namespace OpenBox.Application.Handlers.Brands.Commands;

public class CreateBrandHandler : ICommandHandler<CreateBrand, Guid>
{
    private readonly IBrandRepository _brandRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateBrandHandler(IBrandRepository brandRepository, IUnitOfWork unitOfWork)
    {
        _brandRepository = Guard.Against.Null(brandRepository, nameof(brandRepository));
        _unitOfWork = Guard.Against.Null(unitOfWork, nameof(unitOfWork));
    }

    public async Task<Guid> Handle(CreateBrand command, CancellationToken ct)
    {
        Guard.Against.Null(command, nameof(command));

        var id = _brandRepository.Add(new Brand { Id = Guid.NewGuid(), Name = command.Name });
        await _unitOfWork.SaveChangesAsync(ct);

        return id;
    }
}

public record CreateBrand(
    string Name
);