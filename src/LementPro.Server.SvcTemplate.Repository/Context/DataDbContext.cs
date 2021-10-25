using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LementPro.Server.Common.Repository.Abstract;
using LementPro.Server.Common.Repository.ChangeTracker.Abstract;
using LementPro.Server.Common.Repository.ChangeTracker.Models;
using LementPro.Server.SvcTemplate.Common.Enums;
using LementPro.Server.SvcTemplate.Repository.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Npgsql;


namespace LementPro.Server.SvcTemplate.Repository.Context
{
    /// <inheritdoc/>
    public sealed class DataDbContext : DbContext
    {
        private readonly IContextEntityChangeTracker _changeTracker;
        
        static DataDbContext()
            => NpgsqlConnection.GlobalTypeMapper.MapEnum<BlockWorkStatus>();

        /// <summary>
        /// .ctor
        /// </summary>
        public DataDbContext(DbContextOptions<DataDbContext> options, IContextEntityChangeTracker changeTracker = null) : base(options)
        {
            _changeTracker = changeTracker;
        }

        public DbSet<BlockWorkEntity> BlockWork { get; set; }

        /// <inheritdoc/>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.HasPostgresEnum<BlockWorkStatus>();

            builder.Entity<BlockWorkEntity>(entity =>
            {
                entity.HasIndex(e => new {e.DateCreated, e.Status}).IsUnique(false);
                entity.Property(e=>e.Status).HasConversion(
                    v => v.ToString(),
                    v => (BlockWorkStatus)Enum.Parse(typeof(BlockWorkStatus), v));
            });
        }

        /// <inheritdoc/>
        public override int SaveChanges()
        {
            //ChangeTracker.DetectChanges();

            if (_changeTracker != null && _changeTracker.IsDisabled == false)
            {
                _changeTracker?.ApplyAuditLog(
                    ChangeTracker.Entries()
                        .Where(e => e.State == EntityState.Deleted || e.State == EntityState.Modified || e.State == EntityState.Added)
                        .Select(ConvertToChangeTrackerEntry));
            }

            return base.SaveChanges();
        }

        /// <inheritdoc/>
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            //ChangeTracker.DetectChanges();

            if (_changeTracker != null && _changeTracker.IsDisabled == false)
            {
                _changeTracker?.ApplyAuditLog(
                    ChangeTracker.Entries()
                        .Where(e => e.State == EntityState.Deleted || e.State == EntityState.Modified || e.State == EntityState.Added)
                        .Select(ConvertToChangeTrackerEntry));
            }

            return base.SaveChangesAsync(cancellationToken);
        }

        private static ChangeTrackerEntry ConvertToChangeTrackerEntry(EntityEntry entry) =>
            new ChangeTrackerEntry(entry.Entity as IEntity, entry.State.ToString());
    }
}
