using LementPro.Server.SvcTemplate.Service.Context.Factory;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using LementPro.Server.SvcTemplate.Repository.Abstract;
using LementPro.Server.SvcTemplate.Repository.Extensions;

namespace LementPro.Server.SvcTemplate.Api.Extensions
{
    public static class DbMigrationExtensions
    {
        public static IWebHost MigrateDatabase<TContext>(this IWebHost host) where TContext : DbContext
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var options = services.GetRequiredService<IOptions<Settings.Settings>>().Value;
                using (var dbContext = services.GetRequiredService<IGenericContextFactory<TContext>>().CreateContext())
                {
                    try
                    {
                        if (options.DropOnStart)
                            dbContext.Database.EnsureDeleted();

                        if (dbContext.Database.GetPendingMigrations().Any())
                            dbContext.Database.Migrate();

                        if (options.SeedOnStart)
                        {
                            var seedTemplates = options.SeedTemplate.Split(',', StringSplitOptions.RemoveEmptyEntries);
                            foreach (var template in seedTemplates)
                            {
                                SvcTemplateSeedExtensions.EnsureSeedData(template.Trim(), dbContext);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        //Log errors or do anything you think it's needed
                        throw;
                    }
                }
            }
            return host;
        }
    }
}
