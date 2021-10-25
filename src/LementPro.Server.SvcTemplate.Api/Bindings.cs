using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using LementPro.Server.Common.CorrelationId.Concrete;
using LementPro.Server.SvcTemplate.Repository.Abstract;
using LementPro.Server.SvcTemplate.Repository.Concrete;
using LementPro.Server.SvcTemplate.Repository.Context;
using LementPro.Server.SvcTemplate.Sdk.Abstract;
using LementPro.Server.SvcTemplate.Service.Concrete;
using LementPro.Server.SvcTemplate.Service.Context;
using LementPro.Server.SvcTemplate.Service.Context.ConnectionString;
using LementPro.Server.SvcTemplate.Service.Context.Factory;
using Microsoft.Extensions.Configuration;

namespace LementPro.Server.SvcTemplate.Api
{
    /// <summary>
    /// Class with bindings
    /// </summary>
    public static class Bindings
    {
        /// <summary>
        /// Register types for builder builder
        /// </summary>
        /// <param name="builder">Container</param>
        /// <param name="configuration"></param>
        public static void Bind(ContainerBuilder builder, IConfiguration configuration)
        {
       
            #region Factories

            builder.RegisterGeneric(typeof(GenericContextFactory<>)).As(typeof(IGenericContextFactory<>)).InstancePerLifetimeScope();
            
            #endregion

            
            #region Repository

            builder.RegisterGeneric(typeof(ARepository<>)).As(typeof(IRepository<>)).InstancePerLifetimeScope();
            
            builder.Register(c=> new BlockWorkRepository(c.Resolve<DataDbContext>())).As<IBlockWorkRepository>();

            #endregion


            #region Service

            builder.RegisterType<BlockWorkService>().As<IBlockWorkService>();

            #endregion

            #region Seed

            //builder.RegisterType<SeedTemplateTariff>().Named<ISeedTemplate>("Tariff");

            #endregion


            //builder.RegisterType<RequestContext>().As<IRequestContext>();
        }
    }
}
