using LementPro.Server.Common.EntityFramework.Factories.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Npgsql;
using System.Linq;

namespace LementPro.Server.SvcTemplate.Api.Extensions
{
    public static class DbMigrationExtensions
    {
        public static IHost MigrateDatabase<TContext>(this IHost host) where TContext : DbContext
        {
            using var scope = host.Services.CreateScope();
            
            var services = scope.ServiceProvider;
            var options = services.GetRequiredService<IOptions<Settings.GeneralSettings>>().Value;
           
            using var dbContext = services.GetRequiredService<IGenericContextFactory<TContext>>().CreateNpgsqlContext();
            
            if (options.DropOnStart)
                dbContext.Database.EnsureDeleted();

            if (dbContext.Database.GetPendingMigrations().Any())
                dbContext.Database.Migrate();

            using var conn = (NpgsqlConnection)dbContext.Database.GetDbConnection();
            conn.Open();
            conn.ReloadTypes();

            return host;
        }

        public static IHost OnlyMigrateDatabase<TContext>(this IHost host) where TContext : DbContext
        {
            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;
            
            using var dbContext = services.GetRequiredService<IGenericContextFactory<TContext>>().CreateNpgsqlContext();
            if (dbContext.Database.GetPendingMigrations().Any())
                dbContext.Database.Migrate();

            using var conn = (NpgsqlConnection)dbContext.Database.GetDbConnection();
            conn.Open();
            conn.ReloadTypes();

            return host;
        }
    }
}
