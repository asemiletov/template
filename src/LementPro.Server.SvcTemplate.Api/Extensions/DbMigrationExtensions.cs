using LementPro.Server.Common.EntityFramework.Factories.Abstract;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Linq;

namespace LementPro.Server.SvcTemplate.Api.Extensions
{
    public static class DbMigrationExtensions
    {
        public static IWebHost MigrateDatabase<TContext>(this IWebHost host) where TContext : DbContext
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var options = services.GetRequiredService<IOptions<Settings.GeneralSettings>>().Value;
                using (var dbContext = services.GetRequiredService<IGenericContextFactory<TContext>>().CreateNpgsqlContext())
                {
                    if (options.DropOnStart)
                        dbContext.Database.EnsureDeleted();

                    if (dbContext.Database.GetPendingMigrations().Any())
                        dbContext.Database.Migrate();
                }
            }
            return host;
        }
    }
}
