using System.Linq;
using System.Threading.Tasks;
using LementPro.Server.Common.Repository.Abstract;

namespace LementPro.Server.SvcTemplate.Repository.Abstract
{
    public interface IRepository { }

    // ReSharper disable once InconsistentNaming
    public interface IRepository<TEntity> : IRepository where TEntity : IEntity
    {
        /// <summary>
        /// Get IQueryable DbSet of T
        /// </summary>
        /// <returns></returns>
        IQueryable<TEntity> GetAll();

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
        /// Save changes
        /// </summary>
        /// <returns></returns>
        Task SaveAsync();
    }
}
