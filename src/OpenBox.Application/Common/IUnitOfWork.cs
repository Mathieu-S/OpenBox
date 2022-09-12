namespace OpenBox.Application.Common;

public interface IUnitOfWork
{
    Task SaveChangesAsync();
}