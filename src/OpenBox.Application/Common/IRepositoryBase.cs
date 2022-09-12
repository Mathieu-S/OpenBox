namespace OpenBox.Application.Common;

/// <summary>
/// A generic repository for CRUD on an entity.
/// </summary>
/// <typeparam name="T">The type of entity.</typeparam>
public interface IRepositoryBase<T>
{
    /// <summary>
    /// Get all entity type.
    /// </summary>
    /// <remarks>
    /// The entities are not tracked.
    /// </remarks>
    /// <returns>A list of entity.</returns>
    Task<IEnumerable<T>> GetAllAsync();

    /// <summary>
    /// Get all entity type with pagination system.
    /// </summary>
    /// <remarks>
    /// The entities are not tracked.
    /// </remarks>
    /// <param name="pageIndex">The number of page.</param>
    /// <param name="pageSize">The number of elements.</param>
    /// <returns>A list of entity.</returns>
    Task<IEnumerable<T>> GetAllAsync(int pageIndex, int pageSize);
    
    /// <summary>
    /// Get a specified entity.
    /// </summary>
    /// <remarks>
    /// The entity is not tracked.
    /// </remarks>
    /// <param name="id">The Guid of entity.</param>
    /// <returns>The specified entity. Return null if the entity is not found.</returns>
    /// <exception cref="ArgumentException">Throw when <see cref="Guid"/> is empty.</exception>
    Task<T?> GetAsync(Guid id);

    /// <summary>
    /// Get a specified entity.
    /// </summary>
    /// <param name="id">The Guid of entity.</param>
    /// <param name="asTracking">Indicate whether the entity should be tracked.</param>
    /// <returns>The specified entity. Return null if the entity is not found.</returns>
    /// <exception cref="ArgumentException">Throw when <see cref="Guid"/> is empty.</exception>
    Task<T?> GetAsync(Guid id, bool asTracking);

    /// <summary>
    /// Add an entity.
    /// </summary>
    /// <param name="entity">The entity to add.</param>
    /// <returns>The Guid of created entity.</returns>
    /// <exception cref="ArgumentNullException">Throw when the given entity is null.</exception>
    Guid Add(T entity);

    /// <summary>
    /// Add a list of entities.
    /// </summary>
    /// <param name="entities">The list of entities to add.</param>
    /// <returns>A list of Guid of created entities.</returns>
    /// <exception cref="ArgumentNullException">Throw when the given list of entity is null.</exception>
    /// <exception cref="ArgumentException">Throw when the given list of entity is empty.</exception>
    IEnumerable<Guid> Add(IEnumerable<T> entities);

    /// <summary>
    /// Update an entity.
    /// </summary>
    /// <param name="entity">The entity to update.</param>
    /// <exception cref="ArgumentNullException">Throw when the given entity is null.</exception>
    void Update(T entity);

    /// <summary>
    /// Update a list of entities.
    /// </summary>
    /// <param name="entities">The list of entities to update.</param>
    /// <exception cref="ArgumentNullException">Throw when the given list of entity is null.</exception>
    /// <exception cref="ArgumentException">Throw when the given list of entity is empty.</exception>
    void Update(IEnumerable<T> entities);

    /// <summary>
    /// Delete an entity.
    /// </summary>
    /// <param name="entity">The entity to delete.</param>
    /// <exception cref="ArgumentNullException">Throw when the given entity is null.</exception>
    void Delete(T entity);

    /// <summary>
    /// Delete a list of entities.
    /// </summary>
    /// <param name="entities">The list of entities to delete.</param>
    /// <exception cref="ArgumentNullException">Throw when the given list of entity is null.</exception>
    /// <exception cref="ArgumentException">Throw when the given list of entity is empty.</exception>
    void Delete(IEnumerable<T> entities);
}