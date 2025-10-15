namespace Hospital.Interfaces
{
    /// <summary>
    /// Represents a generic repository interface that defines basic CRUD operations.
    /// This interface can be implemented for various entities, such as <see cref="Patient"/>, <see cref="Doctor"/>, etc.
    /// </summary>
    /// <typeparam name="T">The type of the entity the repository will manage.</typeparam>
    public interface IRepository<T>
    {
        /// <summary>
        /// Creates a new entity asynchronously in the repository.
        /// </summary>
        /// <param name="entity">The entity to be created.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains the ID of the created entity.</returns>
        Task<string> CreateAsync(T entity);

        /// <summary>
        /// Retrieves all entities from the repository asynchronously.
        /// </summary>
        /// <returns>A task representing the asynchronous operation. The task result contains a dictionary of all entities, where the key is the entity's ID.</returns>
        Task<Dictionary<string, T>> GetAllAsync();

        /// <summary>
        /// Retrieves an entity by its ID asynchronously.
        /// </summary>
        /// <param name="id">The ID of the entity to be retrieved.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains the entity if found, or <c>null</c> if not.</returns>
        Task<T> GetByIdAsync(string id);

        /// <summary>
        /// Updates an existing entity in the repository asynchronously.
        /// </summary>
        /// <param name="id">The ID of the entity to be updated.</param>
        /// <param name="entity">The updated entity to replace the old one.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task UpdateAsync(string id, T entity);

        /// <summary>
        /// Deletes an entity from the repository asynchronously.
        /// </summary>
        /// <param name="id">The ID of the entity to be deleted.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task DeleteAsync(string id);
    }
}
