using Autofac;
using Autofac.Extensions.DependencyInjection;
using LementPro.Server.SvcTemplate.Api;
using LementPro.Server.SvcTemplate.Api.Extensions;
using LementPro.Server.SvcTemplate.Repository.Context;
using LementPro.Server.SvcTemplate.Test.Mock;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using System.Collections.Generic;
using System.Reflection;
using LementPro.Server.Common.EntityFramework.Factories.Abstract;
using LementPro.Server.Common.Repository.Seed.Abstract;
using LementPro.Server.SvcTemplate.Repository.Seed.ByServices;
using LementPro.Server.SvcTemplate.Repository.Seed.ByTesters;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace LementPro.Server.SvcTemplate.Test.Base
{
    [SetUpFixture]
    public class BaseTest
    {
        protected IContainer AutofacContainer;

        protected bool DbInMemory { get; set; }

        public BaseTest()
        {
            DbInMemory = false;
        }

        [OneTimeSetUp]
        public virtual void SetUp()
        {
            var configurationBuilder = new ConfigurationBuilder().AddInMemoryCollection(new[]
            {
                new KeyValuePair<string, string>("Specific:A", "B")
            });

            var configuration = configurationBuilder.Build();

            var serviceCollection = new ServiceCollection();
            serviceCollection.RegisterDataContexts(configuration);
            serviceCollection.RegisterConfiguration(configuration);

            serviceCollection.TryAdd(ServiceDescriptor.Singleton<ILoggerFactory, LoggerFactory>());
            serviceCollection.TryAdd(ServiceDescriptor.Singleton(typeof(ILogger<>), typeof(Logger<>)));

            var builder = new ContainerBuilder();
            Bindings.Bind(builder, configuration);
            builder.Populate(serviceCollection);
            InitializeMock(builder);
            AutofacContainer = builder.Build();
        }

        /// <summary>
        /// Register mock.
        /// </summary>
        /// <param name="builder">Container builder</param>
        private void InitializeMock(ContainerBuilder builder)
        {
            builder.RegisterInstance(new LoggerFactory()).As<ILoggerFactory>();

            builder.RegisterInstance(DbInMemory
                ? DbContextOptionsBuilderFactoryMock.GetDbContextOptionsBuilderInMemoryFactory()
                : DbContextOptionsBuilderFactoryMock.GetDbContextOptionsBuilderSqlLiteFactory()).As<IDbContextOptionsBuilderFactory>();

            builder.RegisterType<SeedBlockWorkTemplate>().As<ISeedTemplate>();
            builder.RegisterType<SeedSimpleTemplate>().As<ISeedTemplate>();
        }

        protected DataDbContext GetDbContext()
        {
            var dataContext = AutofacContainer.Resolve<DataDbContext>();
            dataContext.Database.EnsureDeleted();
            dataContext.Database.EnsureCreated();
            return dataContext;
        }

        [OneTimeTearDown]
        public void TearDown()
        {
        }
    }
}
