using Autofac;
using LementPro.Server.Common.EntityFramework.Factories.Abstract;
using LementPro.Server.Common.EntityFramework.Factories.Concrete;
using LementPro.Server.Common.Repository.Abstract;
using LementPro.Server.SvcTemplate.Repository.Abstract;
using LementPro.Server.SvcTemplate.Repository.Concrete;
using LementPro.Server.SvcTemplate.Repository.Context;
using LementPro.Server.SvcTemplate.Repository.Seed;
using LementPro.Server.SvcTemplate.Sdk.Abstract;
using LementPro.Server.SvcTemplate.Service.Concrete;
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

            builder.RegisterType<SeedBaseTemplate>().As<ISeedTemplate>();
            
            #endregion
            
            //builder.RegisterType<RequestContext>().As<IRequestContext>();
        }
    }
}
