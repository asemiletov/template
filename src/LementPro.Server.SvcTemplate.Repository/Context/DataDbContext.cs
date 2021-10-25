using System;
using LementPro.Server.SvcTemplate.Common.Enums;
using LementPro.Server.SvcTemplate.Repository.Entities;
using Microsoft.EntityFrameworkCore;

namespace LementPro.Server.SvcTemplate.Repository.Context
{
    /// <inheritdoc/>
    public sealed class DataDbContext : DbContext
    {
        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="options"></param>
        public DataDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<BlockWorkEntity> BlockWork { get; set; }

        /// <inheritdoc/>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<BlockWorkEntity>(entity =>
            {
                entity.HasIndex(e => new {e.DateCreated, e.Status}).IsUnique(false);
                entity.Property(e=>e.Status).HasConversion(
                    v => v.ToString(),
                    v => (BlockWorkStatus)Enum.Parse(typeof(BlockWorkStatus), v));
            });
        }

        public override int SaveChanges()
        {
            ChangeTracker.DetectChanges();

            return base.SaveChanges();
        }
    }
}
