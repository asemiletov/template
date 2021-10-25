using System;
using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using LementPro.Server.Common.CorrelationId.Middleware;
using LementPro.Server.SvcTemplate.Api.Extensions;
using LementPro.Server.SvcTemplate.Api.Middleware;
using LementPro.Server.SvcTemplate.Api.Settings;
using LementPro.Server.SvcTemplate.Api.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace LementPro.Server.SvcTemplate.Api
{
    public class Startup
    {
        private IHostingEnvironment Environment { get; }
        private IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration, IHostingEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            var enableDatabaseLogging = Environment.IsDevelopment();

            services.RegisterDataContexts(Configuration, enableDatabaseLogging)
                .RegisterConfiguration(Configuration)
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

            var builder = new ContainerBuilder();

            Bindings.Bind(builder, Configuration);

            builder.Populate(services);

            var container = builder.Build();

            //InitDatabase(container);

            return new AutofacServiceProvider(container);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
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

            app.UseCors();

            app.UseMiddleware<CorrelationIdMiddleware>();
            app.UseMiddleware<ErrorHandlingMiddleware>();
            
            app.UseMvc(routes =>
            {
                routes.MapRoute("default", "{controller=Home}/{action=Index}");
            });
        }
    }
}
