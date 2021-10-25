using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using LementPro.Server.Common.Repository.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace LementPro.Server.SvcTemplate.Repository.Abstract
{
    /// <inheritdoc cref="IRepository{TEntity}"/>
    public abstract class ARepository<TEntity> : IRepository<TEntity> where TEntity : AEntity
    {
        private readonly DbContext _context;
        protected readonly DbSet<TEntity> _dbset;

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="context"></param>
        protected ARepository(DbContext context)
        {
            _context = context;
            _dbset = context.Set<TEntity>();
        }

        /// <inheritdoc />
        public async Task<TEntity> FindAsync(object id) => await _dbset.FindAsync(id);

        /// <inheritdoc />
        public virtual async Task<TEntity> Add(TEntity entity)
        {
            var x = await _dbset.AddAsync(entity);

            return x.Entity;
        }

        /// <inheritdoc />
        public virtual TEntity Delete(TEntity entity) => _dbset.Remove(entity).Entity;

        /// <inheritdoc />
        public virtual void Edit(TEntity entity) => _context.Entry(entity).State = EntityState.Modified;

        /// <inheritdoc />
        public virtual Task AddRangeAsync(IEnumerable<TEntity> entities) => _dbset.AddRangeAsync(entities);

        /// <inheritdoc />
        public virtual Task SaveAsync(CancellationToken cancellationToken = default) => _context.SaveChangesAsync(cancellationToken);

        /// <inheritdoc />
        public virtual Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default) => _context.Database.BeginTransactionAsync(cancellationToken);

        public virtual void BulkDelete(IEnumerable<TEntity> entities) => _context.RemoveRange(entities);

        public virtual void BulkUpdate(IEnumerable<TEntity> entities) => _context.UpdateRange(entities);

        #region IQueryable

        private IQueryable<TEntity> Query => _dbset;

        public Type ElementType => typeof(TEntity);

        public Expression Expression => Query.Expression;

        public IQueryProvider Provider => Query.Provider;

        public IEnumerator<TEntity> GetEnumerator() => Query.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        #endregion

        public IAsyncEnumerator<TEntity> GetAsyncEnumerator(CancellationToken cancellationToken = new CancellationToken()) =>
            _dbset.AsAsyncEnumerable().GetAsyncEnumerator(cancellationToken);
    }
}