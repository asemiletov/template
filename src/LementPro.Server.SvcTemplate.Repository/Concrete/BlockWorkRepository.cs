using System;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using LementPro.Server.SvcTemplate.Repository.Abstract;
using LementPro.Server.SvcTemplate.Repository.Entities;
using Microsoft.EntityFrameworkCore;

namespace LementPro.Server.SvcTemplate.Repository.Concrete
{
    /// <summary>
    /// BlockWork repository
    /// </summary>
    public class BlockWorkRepository : ARepository<BlockWorkEntity>, IBlockWorkRepository
    {
        public BlockWorkRepository(DbContext context) : base(context)
        {
        }

        public override Task<BlockWorkEntity> Add(BlockWorkEntity entity)
        {
            entity.DateCreated = DateTimeOffset.UtcNow;
            entity.DateUpdated= DateTimeOffset.UtcNow;
            return base.Add(entity);
        }

        public override void Edit(BlockWorkEntity entity)
        {
            entity.DateUpdated = DateTimeOffset.UtcNow;
            base.Edit(entity);
        }
    }
}
