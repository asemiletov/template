using System;
using System.IO;
using System.Linq;
using System.Reflection;
using LementPro.Server.SvcTemplate.Api.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace LementPro.Server.SvcTemplate.Api.Swagger
{
    /// <summary>
    /// Helper class to initialize and configure swagger
    /// </summary>
    public static class SwaggerHelper
    {
        private const string ConfigFile = "swaggersettings.json";

        /// <summary>
        /// Swagger configuration
        /// </summary>
        private static SwaggerConfiguration Configuration => ConfigurationLazy.Value;

        private static readonly Lazy<SwaggerConfiguration> ConfigurationLazy = new Lazy<SwaggerConfiguration>(() =>
        {
            // load from config file 
            var builder = new ConfigurationBuilder().AddJsonFile(ConfigFile).Build();

            var configuration = new SwaggerConfiguration();
            builder.GetSection("Swagger").Bind(configuration, o => o.BindNonPublicProperties = true);

            return configuration;
        });

        /// <summary>
        /// Configure Swagger
        /// </summary>
        /// <param name="swaggerOptions">Swagger options</param>
        public static void ConfigureSwagger(SwaggerOptions swaggerOptions)
        {
            swaggerOptions.RouteTemplate = "swagger/{documentName}/swagger.json";
        }


        /// <summary>
        /// Configure SwaggerUI
        /// </summary>
        /// <param name="swaggerOptions">SwaggerUI options</param>
        public static void ConfigureSwagger(SwaggerUIOptions swaggerOptions)
        {
            foreach (var doc in Configuration)
                doc.Sections?.ToList().ForEach(section => swaggerOptions.SwaggerEndpoint($"/swagger/{section.Name}/swagger.json", $"{section.Name}"));

            swaggerOptions.RoutePrefix = "swagger";
        }

        /// <summary>
        /// Configure Swagger Gen
        /// </summary>
        /// <param name="swaggerGenOptions">SwaggerGen options</param>
        public static void ConfigureSwaggerGen(SwaggerGenOptions swaggerGenOptions)
        {
            foreach (var doc in Configuration)
            {
                var assemblyInfo = Assembly.Load(doc.Assembly).GetName();

                doc.Sections?.ToList().ForEach(section =>
                    swaggerGenOptions.SwaggerDoc($"{section.Name}", new Info
                    {
                        Title = $"{assemblyInfo.Name}",
                        Version = $"{assemblyInfo.Version.Major}.{assemblyInfo.Version.Minor}",
                        Description = section.Description,
                    }));


                var fileInfo = new FileInfo(Path.Combine(AppContext.BaseDirectory, $"{assemblyInfo.Name}.xml"));
                if (fileInfo.Exists)
                    swaggerGenOptions.IncludeXmlComments(fileInfo.FullName);
            }

            swaggerGenOptions.IgnoreObsoleteActions();
            swaggerGenOptions.DescribeAllEnumsAsStrings();
        }

    }
}
