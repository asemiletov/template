using System;
using System.Linq;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using LementPro.Server.Common.CorrelationId.Middleware;
using LementPro.Server.SvcTemplate.Api.Extensions;
using LementPro.Server.SvcTemplate.Api.Middleware;
using LementPro.Server.SvcTemplate.Api.Swagger;
using LementPro.Server.SvcTemplate.Repository.Context;
using LementPro.Server.SvcTemplate.Service.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

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
            services.RegisterDataContexts(Configuration)
                .RegisterConfiguration(Configuration)
                .ConfigureCommonServices();

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

                // обработка ошибок HTTP
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
