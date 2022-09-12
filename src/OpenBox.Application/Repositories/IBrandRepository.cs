using OpenBox.Application.Common;
using OpenBox.Domain.Entities;

namespace OpenBox.Application.Repositories;

/// <summary>
/// The repository of <see cref="Brand"/> entity.
/// </summary>
public interface IBrandRepository : IRepositoryBase<Brand>
{
    /// <summary>
    /// Find a Brand by its name.
    /// </summary>
    /// <remarks>
    /// The entity is not tracked.
    /// </remarks>
    /// <param name="name">The name of brand.</param>
    /// <returns>The brand name requested.</returns>
    /// <exception cref="ArgumentNullException">Throw when the given name is null.</exception>
    /// <exception cref="ArgumentException">Throw when the given name is empty or white space string.</exception>
    Task<Brand?> GetByNameAsync(string name);

    /// <summary>
    /// Find a Brand by its name.
    /// </summary>
    /// <param name="name">The name of brand.</param>
    /// <param name="asTracking">Indicate whether the entity should be tracked.</param>
    /// <returns>The brand name requested.</returns>
    /// <exception cref="ArgumentNullException">Throw when the given name is null.</exception>
    /// <exception cref="ArgumentException">Throw when the given name is empty or white space string.</exception>
    Task<Brand?> GetByNameAsync(string name, bool asTracking);
}