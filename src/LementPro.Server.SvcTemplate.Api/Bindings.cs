using System;
using Autofac;
using LementPro.Server.Common.ActionLog.DatabaseProcessor.Repository.Context;
using LementPro.Server.Common.EntityFramework.Factories.Abstract;
using LementPro.Server.Common.EntityFramework.Factories.Concrete;
using LementPro.Server.Common.Repository.ChangeTracker.Abstract;
using LementPro.Server.Common.Repository.Seed.Abstract;
using LementPro.Server.Common.Repository.Seed.Concrete;
using LementPro.Server.SvcTemplate.Api.Adapters.User;
using LementPro.Server.SvcTemplate.Repository.Abstract;
using LementPro.Server.SvcTemplate.Repository.Concrete;
using LementPro.Server.SvcTemplate.Repository.Context;
using LementPro.Server.SvcTemplate.Repository.Seed;
using LementPro.Server.SvcTemplate.Sdk.Abstract;
using LementPro.Server.SvcTemplate.Service.Abstract;
using LementPro.Server.SvcTemplate.Service.Concrete;
using Microsoft.EntityFrameworkCore;
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
            builder.Register<Func<DbContextOptions, DataDbContext>>(c =>
            {
                var changeTracker = c.Resolve<IContextEntityChangeTracker>();
                return (contextOptions) => new DataDbContext(contextOptions as DbContextOptions<DataDbContext>, changeTracker);
                //return (contextOptions) => new DataDbContext(contextOptions as DbContextOptions<DataDbContext>, null);
            });
            builder.Register<Func<DbContextOptions, ActionLogDbContext>>(c =>(contextOptions) => new ActionLogDbContext(contextOptions as DbContextOptions<ActionLogDbContext>));

            builder.RegisterType<DatabaseSeedFactory>().As<IDatabaseSeedFactory>();
            
            #endregion
            
            #region Repository

            builder.RegisterGeneric(typeof(ARepository<>)).As(typeof(IRepository<>)).InstancePerLifetimeScope();
            
            builder.Register(c=> new BlockWorkRepository(c.Resolve<DataDbContext>())).As<IBlockWorkRepository>();

            #endregion
            
            #region Service

            builder.RegisterType<BlockWorkService>().As<IBlockWorkService>();
            
            #endregion

            #region Adapters 

            builder.RegisterType<BlockWorkUserAdapter>().As<IBlockWorkUserAdapter>();

            #endregion

            #region Seed

            builder.RegisterType<SeedBaseTemplate>().As<ISeedTemplate>();
            
            #endregion
            
            //builder.RegisterType<RequestContext>().As<IRequestContext>();
        }
    }
}
