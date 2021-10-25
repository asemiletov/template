using System;
using System.IO;
using System.Reflection;
using System.Text.Json;
using Autofac.Extensions.DependencyInjection;
using LementPro.Server.Common.ActionLog.DatabaseProcessor.Repository.Context;
using LementPro.Server.Common.Serializer.Concrete;
using LementPro.Server.SvcTemplate.Api.Extensions;
using LementPro.Server.SvcTemplate.Api.Settings;
using LementPro.Server.SvcTemplate.Repository.Context;
using LementPro.Server.SvcTemplate.Service.Settings;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NLog;
using NLog.Web;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace LementPro.Server.SvcTemplate.Api
{
    public class Program
    {
        //private static IWebHost _host;

        public static void Main(string[] args)
        {
            var appBasePath = Directory.GetCurrentDirectory();
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            GlobalDiagnosticsContext.Set("appbasepath", appBasePath);
            GlobalDiagnosticsContext.Set("appenv", env);

            var logger = NLogBuilder.ConfigureNLog("NLog.config").GetCurrentClassLogger();
            JsonSerializerOptions options = new JsonSerializerOptions(JsonSerializerDefaults.Web);
            try
            {
                logger.Info($"==============================================================================================================");

                logger.Info($"Start {Assembly.GetExecutingAssembly().GetName().FullName}");
                logger.Info($"ASPNETCORE_ENVIRONMENT: {env}");
                logger.Info($"BasePath: {appBasePath}");

                logger.Info($"==============================================================================================================");

                var host = CreateWebHostBuilder(args).Build();

                using (var scope = host.Services.CreateScope())
                {
                    var services = scope.ServiceProvider;

                    var generalOptions = services.GetRequiredService<IOptions<GeneralSettings>>().Value;

                    logger.Info($"GeneralSettings.TestMode: {generalOptions.TestMode}");
                    logger.Info($"GeneralSettings.DropOnStart: {generalOptions.DropOnStart}");
                    logger.Info($"GeneralSettings.SeedOnStart: {generalOptions.SeedOnStart}");
                    logger.Info($"GeneralSettings.SeedTemplate: {generalOptions.SeedTemplate}");

                    var specificSettings = services.GetRequiredService<IOptions<SpecificSettings>>().Value;

                    logger.Info($"SpecificSettings.Otlp.Enabled: {specificSettings.Otlp.Enabled}");
                    logger.Info($"SpecificSettings.Otlp.Endpoint: {specificSettings.Otlp.Endpoint}");
                }

                host.MigrateDatabase<DataDbContext>()
                    .OnlyMigrateDatabase<ActionLogDbContext>()
                    .Run();
            }
            catch (Exception ex)
            {
                //NLog: catch setup errors
                logger.Error(ex, $"Stopped {Assembly.GetExecutingAssembly().GetName().FullName} because of exception");
                throw;
            }
            finally
            {
                // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
                LogManager.Shutdown();
            }
        }

        private static IHostBuilder CreateWebHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .ConfigureAppConfiguration((hostingContext, configurationBuilder) =>
                {
                    configurationBuilder.AddJsonFile("secrets.json", true, true);
                })
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.SetMinimumLevel(LogLevel.Trace);
                })
                .UseNLog();
    }
}
