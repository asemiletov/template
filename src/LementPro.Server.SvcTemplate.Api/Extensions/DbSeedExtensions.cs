using System;
using System.Linq;
using LementPro.Server.Common.Repository.Abstract;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace LementPro.Server.SvcTemplate.Api.Extensions
{
    public static class DbSeedExtensions
    {
        private static readonly char[] Delimiters = {',', ';', ' '};

        public static IWebHost SeedDatabase(this IWebHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var options = services.GetRequiredService<IOptions<Settings.GeneralSettings>>().Value;
                if (!options.SeedOnStart)
                    return host;

                var seedTemplates = options.SeedTemplate.Split(Delimiters, StringSplitOptions.RemoveEmptyEntries);
                if (!seedTemplates.Any())
                    return host;

                var seedFactory = services.GetRequiredService<IDatabaseSeedFactory>();

                foreach (var tpl in seedTemplates)
                {
                    seedFactory.GetTemplate(tpl)?.Start();
                }
            }
            return host;
        }
    }
}
