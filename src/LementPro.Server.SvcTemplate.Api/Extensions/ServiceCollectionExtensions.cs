using AutoMapper;
using LementPro.Server.Common.CorrelationId.Middleware.Extensions;
using LementPro.Server.Common.Sdk.Models;
using LementPro.Server.SvcTemplate.Api.Swagger;
using LementPro.Server.SvcTemplate.Repository.Context;
using LementPro.Server.SvcTemplate.Service.Mapping;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Converters;
using System.Linq;
using System.Reflection;
using LementPro.Server.Common.EntityFramework;
using LementPro.Server.Common.EntityFramework.ConnectionString.Abstract;
using LementPro.Server.Common.EntityFramework.ConnectionString.Concrete;
using LementPro.Server.Common.EntityFramework.Factories.Abstract;
using LementPro.Server.Common.EntityFramework.Factories.Concrete;
using LementPro.Server.SvcTemplate.Api.Settings;

namespace LementPro.Server.SvcTemplate.Api.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void ConfigureCommonServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSwaggerGen(SwaggerHelper.ConfigureSwaggerGen);

            serviceCollection.AddAutoMapper(
                Assembly.GetExecutingAssembly(),
                Assembly.GetAssembly(typeof(MappingProfile)));

            serviceCollection.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddJsonOptions(MvcJsonOptions);

            serviceCollection.RegisterCorrelationIdTypes();

            serviceCollection.AddCors(CorsOptions);

            serviceCollection.Configure<ApiBehaviorOptions>(ApiBehaviorOptions);

            serviceCollection.AddResponseCompression(options =>
            {
                options.Providers.Add<GzipCompressionProvider>();
            });
        }

        public static IServiceCollection RegisterDataContexts(this IServiceCollection services, IConfiguration configuration, bool enableLogging = false) =>
            services.AddSingleton<IConnectionStringProvider,ConnectionStringProvider>()
                    .AddSingleton<IDbContextOptionsBuilderFactory, DbContextOptionsBuilderFactory>()
                    .AddDbContext<DataDbContext>((c, builder) =>
                    {
                        if (enableLogging)
                        {
                            builder.EnableSensitiveDataLogging(true);
                            builder.EnableDetailedErrors(true);
                            builder.UseLoggerFactory(c.GetRequiredService<ILoggerFactory>());
                        }

                        c.GetRequiredService<IDbContextOptionsBuilderFactory>().CreateNpgsql<DataDbContext>(builder);
                    },ServiceLifetime.Scoped);

        public static IServiceCollection RegisterConfiguration(this IServiceCollection serviceCollection, IConfiguration configuration)=>
            serviceCollection.Configure<ConnectionStringsSettings>(configuration.GetSection("ConnectionStrings"))
                             .Configure<Settings.GeneralSettings>(configuration.GetSection("General"), o => o.BindNonPublicProperties = true)
                             .Configure<Settings.SpecificSettings>(configuration.GetSection("Specific"), o => o.BindNonPublicProperties = true);
        //serviceCollection.Configure<SwaggerConfiguration>(configuration.GetSection("Swagger"));
//        serviceCollection
  

        private static void MvcJsonOptions(MvcJsonOptions options)
        {
            options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            options.SerializerSettings.Converters.Add(new StringEnumConverter());
        }

        private static void CorsOptions(CorsOptions options)
        {
            options.AddDefaultPolicy(cors =>
            {
                cors.AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowAnyOrigin();
            });
        }

        private static void ApiBehaviorOptions(ApiBehaviorOptions options)
        {
            options.InvalidModelStateResponseFactory = actionContext => new BadRequestObjectResult(new ValidationResponse()
            {
                Message = "Validation fail",
                Errors = actionContext.ModelState.Keys.SelectMany(key => actionContext.ModelState[key].Errors.Select(x => new ValidationResponseItem(key, x.ErrorMessage))).ToList()
            });
        }
    }
}
