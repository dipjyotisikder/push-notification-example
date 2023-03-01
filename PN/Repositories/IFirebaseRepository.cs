using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Google.Cloud.Firestore;
using PN.Firestore.Entities;

namespace PN.Firestore.Repositories;

/// <summary>
/// Represents a firestore repository.
/// </summary>
/// <typeparam name="T">Entity Type.</typeparam>
public interface IFirebaseRepository<T>
    where T : IEntity
{
    /// <summary>
    /// Gets all record from the repository.
    /// </summary>
    /// <returns>a records of type T.</returns>
    Task<List<T>> GetAllAsync();

    /// <summary>
    /// Gets a record from the repository.
    /// </summary>
    /// <param name="entityId">Db entity id.</param>
    /// <returns>a record of type T.</returns>
    Task<T> GetAsync(string entityId);

    /// <summary>
    /// Adds a record to the repository.
    /// </summary>
    /// <param name="entity">Db entity.</param>
    /// <returns>A record of type T.</returns>
    Task<T> AddAsync(T entity);

    /// <summary>
    /// Updates a record in the repository.
    /// </summary>
    /// <param name="entity">Db entity.</param>
    /// <returns>A record of type T.</returns>
    Task<T> UpdateAsync(T entity);

    /// <summary>
    /// Adds a record to the repository.
    /// </summary>
    /// <param name="entity">Db entity.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns> S
    Task DeleteAsync(T entity);

    /// <summary>
    /// Query all record from the repository.
    /// </summary>
    /// <param name="action">Query.</param>
    /// <returns>a records of type T.</returns>
    Task<List<T>> QueryRecordsAsync(Func<CollectionReference, Query> action);
}
