using LementPro.Server.SvcTemplate.Service.Context.ConnectionString;
using Microsoft.EntityFrameworkCore;
using System;
using System.Reflection;

namespace LementPro.Server.SvcTemplate.Service.Context.Factory
{

    public interface IDbContextOptionsBuilderFactory
    {
        DbContextOptionsBuilder Create<TContext>(DbContextOptionsBuilder builder = null) where TContext : DbContext;
    }

    public class DbContextOptionsBuilderFactory : IDbContextOptionsBuilderFactory
    {
        private readonly IConnectionStringProvider _connectionStringProvider;

        public DbContextOptionsBuilderFactory(IConnectionStringProvider connectionStringProvider)
        {
            _connectionStringProvider = connectionStringProvider;
        }

        public DbContextOptionsBuilder Create<TContext>(DbContextOptionsBuilder builder = null) where TContext : DbContext
        {
            var optionsBuilder = builder ?? new DbContextOptionsBuilder<TContext>();
            optionsBuilder.UseNpgsql(_connectionStringProvider.GetConnectionString(typeof(TContext).Name), t =>
            {
                t.MigrationsAssembly(Assembly.GetAssembly(typeof(TContext)).FullName);
                t.MaxBatchSize(500);
            });

            return optionsBuilder;
        }
    }
}