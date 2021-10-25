using LementPro.Server.Common.ActionLog.Abstract;
using LementPro.Server.Common.ActionLog.DatabaseProcessor;
using LementPro.Server.Common.ActionLog.LoggerProcessor;
using LementPro.Server.Common.ActionLog.Middleware.Extensions;
using LementPro.Server.Common.Authentication.Middleware;
using LementPro.Server.Common.Context.Middleware.Extensions;
using LementPro.Server.Common.EntityFramework;
using LementPro.Server.Common.EntityFramework.ConnectionString.Abstract;
using LementPro.Server.Common.EntityFramework.ConnectionString.Concrete;
using LementPro.Server.Common.EntityFramework.Factories.Abstract;
using LementPro.Server.Common.EntityFramework.Factories.Concrete;
using LementPro.Server.Common.Sdk.Models;
using LementPro.Server.Common.Token.Settings;
using LementPro.Server.SvcTemplate.Api.Swagger;
using LementPro.Server.SvcTemplate.Repository.Context;
using LementPro.Server.SvcTemplate.Service.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Linq;
using LementPro.Server.Common.ActionLog.DatabaseProcessor.Repository.Context;
using LementPro.Server.Common.Serializer.Concrete;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;

namespace LementPro.Server.SvcTemplate.Api.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void ConfigureCommonServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSwaggerGen(SwaggerHelper.ConfigureSwaggerGen);

            serviceCollection
                .AddControllers()
                .AddJsonOptions(jsonOptions =>
                {
                    jsonOptions.JsonSerializerOptions.IgnoreNullValues = JsonHelper.DefaultOptions.IgnoreNullValues;
                    jsonOptions.JsonSerializerOptions.PropertyNameCaseInsensitive = JsonHelper.DefaultOptions.PropertyNameCaseInsensitive;
                    jsonOptions.JsonSerializerOptions.PropertyNamingPolicy = JsonHelper.DefaultOptions.PropertyNamingPolicy;
                    jsonOptions.JsonSerializerOptions.NumberHandling = JsonHelper.DefaultOptions.NumberHandling;
                    foreach (var defaultOptionsConverter in JsonHelper.DefaultOptions.Converters)
                    {
                        jsonOptions.JsonSerializerOptions.Converters.Add(defaultOptionsConverter);
                    }
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            serviceCollection.RegisterRequestContextTypes();

            serviceCollection.AddCors(CorsOptions);

            serviceCollection.Configure<ApiBehaviorOptions>(ApiBehaviorOptions);

            serviceCollection.TryAdd(ServiceDescriptor.Singleton<ILoggerFactory, LoggerFactory>());
            serviceCollection.TryAdd(ServiceDescriptor.Singleton(typeof(ILogger<>), typeof(Logger<>)));

            serviceCollection.AddResponseCompression(options =>
            {
                options.Providers.Add<GzipCompressionProvider>();
            });

            serviceCollection.AddControllersWithViews();
            serviceCollection.AddRazorPages();
        }

        public static IServiceCollection RegisterDataContexts(this IServiceCollection services, IConfiguration configuration, bool enableLogging = false) =>
            services.AddSingleton<IConnectionStringProvider, ConnectionStringProvider>()
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
                    }, ServiceLifetime.Scoped);

        public static IServiceCollection RegisterConfiguration(this IServiceCollection serviceCollection,
            IConfiguration configuration) =>
            serviceCollection.Configure<ConnectionStringsSettings>(configuration.GetSection("ConnectionStrings"))
                .Configure<Settings.GeneralSettings>(configuration.GetSection("General"), o => o.BindNonPublicProperties = true)
                .Configure<SpecificSettings>(configuration.GetSection("Specific"))
                .Configure<TokenServiceSettings>(options =>
                {
                    options.ValidationParameters ??= new TokenValidationParameters();
                    TokenValidationDefaults.Internal.Invoke(options.ValidationParameters);
                    configuration.GetSection("Specific:TokenSettings").Bind(options);
                });
        
        public static IServiceCollection RegisterAuthenticationServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddAuthorization(options =>
            {
                options.DefaultPolicy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme).RequireAuthenticatedUser().Build();
            });
            serviceCollection.AddAuthentication(options => { options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme; }).AddJwtBearer();
            serviceCollection.AddSingleton<IPostConfigureOptions<JwtBearerOptions>, ConfigureJwtBearerOptions>();

            return serviceCollection;
        }

        public static IServiceCollection RegisterActionLogServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.RegisterActionLogTypes();

            serviceCollection.AddScoped<IActionLogProcessor, ActionLogDatabaseProcessor>();
            serviceCollection.AddScoped<IActionLogProcessor, ActionLogLoggerProcessor>();

            serviceCollection.AddDbContext<ActionLogDbContext>((c, builder) =>
                c.GetRequiredService<IDbContextOptionsBuilderFactory>().CreateNpgsql<ActionLogDbContext>(builder));

            return serviceCollection;
        }

        #region private

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

        #endregion
    }
}
