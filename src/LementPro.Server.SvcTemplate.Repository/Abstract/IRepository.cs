using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LementPro.Server.Common.Repository.Abstract;
using Microsoft.EntityFrameworkCore.Storage;

namespace LementPro.Server.SvcTemplate.Repository.Abstract
{
    public interface IRepository { }

    // ReSharper disable once InconsistentNaming
    public interface IRepository<TEntity> : IRepository, IQueryable<TEntity>, IAsyncEnumerable<TEntity> where TEntity : IEntity
    {
        /// <summary>
        /// Find by PK
        /// </summary>
        /// <param name="id">Entity primary key</param>
        /// <returns></returns>
        Task<TEntity> FindAsync(object id);

        /// <summary>
        /// Add entity
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<TEntity> Add(TEntity entity);

        /// <summary>
        /// Add range of entities
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task AddRangeAsync(IEnumerable<TEntity> entities);

        /// <summary>
        /// Edit entity
        /// </summary>
        /// <param name="entity"></param>
        void Edit(TEntity entity);

        /// <summary>
        /// Delete entity
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        TEntity Delete(TEntity entity);

        /// <summary>
        /// Bulk delete
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        void BulkDelete(IEnumerable<TEntity> entities);

        /// <summary>
        /// Bulk update
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        void BulkUpdate(IEnumerable<TEntity> entities);

        /// <summary>
        /// Save changes
        /// </summary>
        /// <returns></returns>
        Task SaveAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Begin transaction
        /// </summary>
        /// <returns></returns>
        Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
    }
}