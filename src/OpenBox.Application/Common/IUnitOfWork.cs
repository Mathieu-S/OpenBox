using OpenBox.Application.Exceptions;

namespace OpenBox.Application.Common;

/// <summary>
/// Represent a manager for database context.
/// </summary>
public interface IUnitOfWork
{
    /// <summary>
    /// Save the main database context.
    /// </summary>
    /// <exception cref="ContextCannotBeSavedException">Throw when the context cannot be saved. See exception for more information.</exception>
    /// <returns></returns>
    Task SaveChangesAsync();
    
    /// <summary>
    /// Save the main database context.
    /// </summary>
    /// <param name="ct">A <see cref="CancellationToken"/> to observe while waiting for the task to complete</param>
    /// <exception cref="ContextCannotBeSavedException">Throw when the context cannot be saved. See exception for more information.</exception>
    /// <returns></returns>
    Task SaveChangesAsync(CancellationToken ct);
}