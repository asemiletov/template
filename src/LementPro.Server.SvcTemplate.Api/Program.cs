using System;
using System.IO;
using System.Reflection;
using LementPro.Server.SvcTemplate.Api.Extensions;
using LementPro.Server.SvcTemplate.Repository.Context;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
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

            try
            {
                logger.Info($"==============================================================================================================");

                logger.Info($"Start {Assembly.GetExecutingAssembly().GetName().FullName}");
                logger.Info($"ASPNETCORE_ENVIRONMENT: {env}");
                logger.Info($"BasePath: {appBasePath}");

                logger.Info($"==============================================================================================================");

                CreateWebHostBuilder(args)
                                .Build()
                                .MigrateDatabase<DataDbContext>()
                                .Run();
            }
            catch (Exception ex)
            {
                //NLog: catch setup errors
                logger.Error(ex, "Stopped Bridge.Universalna.Api because of exception");
                throw;
            }
            finally
            {
                // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
                LogManager.Shutdown();
            }
        }

        private static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .ConfigureAppConfiguration((hostingContext, configurationBuilder) =>
                {
                    configurationBuilder.AddCommandLine(args);
                })
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.SetMinimumLevel(LogLevel.Trace);
                })
                .UseNLog();
    }
}
