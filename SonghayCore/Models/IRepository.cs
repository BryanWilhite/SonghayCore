using System.Collections.Generic;

namespace Songhay.Models
{
    /// <summary>
    /// Defines the repository pattern for an Entity.
    /// </summary>
    /// <remarks>
    /// Based on the NBlog repository by Chris Fulstow
    /// [https://github.com/ChrisFulstow/NBlog/blob/master/NBlog.Web/Application/Storage/IRepository.cs]
    /// </remarks>
    public interface IRepository
    {
        /// <summary>
        /// Deletes the entity.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="key">The key.</param>
        void DeleteEntity<TEntity>(object key) where TEntity : class, new();

        /// <summary>
        /// Determines whether the specified key has entity.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="key">The key.</param>
        bool HasEntity<TEntity>(object key) where TEntity : class, new();

        /// <summary>
        /// Loads all.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        IEnumerable<TEntity> LoadAll<TEntity>() where TEntity : class, new();

        /// <summary>
        /// Loads the single.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="key">The key.</param>
        TEntity LoadSingle<TEntity>(object key) where TEntity : class, new();

        /// <summary>
        /// Saves the entity.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="item">The item.</param>
        void SaveEntity<TEntity>(TEntity item) where TEntity : class, new();
    }
}
