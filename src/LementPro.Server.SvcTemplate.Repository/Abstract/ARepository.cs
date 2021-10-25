using System.Linq;
using System.Threading.Tasks;
using LementPro.Server.Common.Repository.Abstract;
using Microsoft.EntityFrameworkCore;

namespace LementPro.Server.SvcTemplate.Repository.Abstract
{

    public abstract class ARepository<TEntity> : IRepository<TEntity> where TEntity : AEntity
    {
        private readonly DbContext _context;
        private readonly DbSet<TEntity> _dbset;

        protected ARepository(DbContext context)
        {
            _context = context;
            _dbset = context.Set<TEntity>();
        }

        public virtual IQueryable<TEntity> GetAll()
        {
            return _dbset;
        }
        
        public Task<TEntity> FindAsync(object id)
        {
            return _dbset.FindAsync(id);
        }
        
        public virtual async Task<TEntity> Add(TEntity entity)
        {
            var x = await _dbset.AddAsync(entity);

            return x.Entity;
        }

        public virtual TEntity Delete(TEntity entity)
        {
            return _dbset.Remove(entity).Entity;
        }

        public virtual void Edit(TEntity entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }

        public virtual async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
