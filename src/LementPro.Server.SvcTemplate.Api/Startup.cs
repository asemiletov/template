using System;
using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using LementPro.Server.Common.ActionLog.Middleware;
using LementPro.Server.Common.Context.Middleware;
using LementPro.Server.SvcTemplate.Api.Extensions;
using LementPro.Server.SvcTemplate.Api.Middleware;
using LementPro.Server.SvcTemplate.Api.Swagger;
using LementPro.Server.SvcTemplate.Service.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace LementPro.Server.SvcTemplate.Api
{
    public class Startup
    {
        private IWebHostEnvironment Environment { get; }
        private IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var enableDatabaseLogging = Environment.IsDevelopment();

            services.RegisterDataContexts(Configuration, enableDatabaseLogging)
                .RegisterConfiguration(Configuration)
                .RegisterAuthenticationServices()
                .RegisterActionLogServices()
                .ConfigureCommonServices();

            #region openTelemetry

            {
                var otlsSettings = Configuration.GetSection("Specific:Otlp").Get<OtlpSettings>();

                if (otlsSettings.Enabled)
                {
                    var assembly = Assembly.GetEntryAssembly()?.GetName();

                    // Adding the OtlpExporter creates a GrpcChannel.
                    // This switch must be set before creating a GrpcChannel/HttpClient when calling an insecure gRPC service.
                    // See: https://docs.microsoft.com/aspnet/core/grpc/troubleshoot#call-insecure-grpc-services-with-net-core-client
                    AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

                    services.AddOpenTelemetryTracing((otlpBuilder) => otlpBuilder
                        .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService($"{assembly?.Name}"))
                        .AddAspNetCoreInstrumentation(config => {
                            config.RecordException = true;
                            config.Enrich = (activity, eventName, rawObject) =>
                            {
                                activity.SetTag("env", Environment.EnvironmentName);
                                activity.SetTag("version", assembly?.Version);
                            };
                        })
                        .AddHttpClientInstrumentation()
                        .AddOtlpExporter(otlpOptions =>
                        {
                            otlpOptions.Endpoint = new Uri(otlsSettings.Endpoint);
                        }));
                }
            }

            #endregion
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            Bindings.Bind(builder, Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //app.UseMiddleware(typeof(ErrorHandlingMiddleware));
            }
            else
            {
                app.UseExceptionHandler("/error");

                // Once a supported browser receives this header that browser will prevent any communications from being sent over HTTP to the specified domain and will instead send all communications over HTTPS. 
                // It also prevents HTTPS click through prompts on browsers.
                //app.UseHsts();

                // HTTP errors handling
                app.UseStatusCodePagesWithReExecute("/error/{0}");
            }

            //app.UseHttpsRedirection();
            app.UseSwagger(SwaggerHelper.ConfigureSwagger);

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwaggerUI(SwaggerHelper.ConfigureSwagger);
            
            app.UseMiddleware<HttpRequestContextMiddleware>();
            app.UseMiddleware<ActionLogMiddleware>();
            app.UseMiddleware<ErrorHandlingMiddleware>();
            
            app.UseStaticFiles();
            app.UseRouting();
            app.UseCors();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(routes =>
            {
                routes.MapDefaultControllerRoute();
                routes.MapRazorPages();
            });
        }
    }
}
