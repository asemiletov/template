using System;
using System.IO;
using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Autofac.Extras.FakeItEasy;
using AutoMapper;
using LementPro.Server.SvcTemplate.Api;
using LementPro.Server.SvcTemplate.Api.Extensions;
using LementPro.Server.SvcTemplate.Sdk.Clients;
using LementPro.Server.SvcTemplate.Service.Context.Factory;
using LementPro.Server.SvcTemplate.Service.Mapping;
using LementPro.Server.SvcTemplate.Test.Mock;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NUnit.Framework;

namespace LementPro.Server.SvcTemplate.Test.Base
{
    [SetUpFixture]
    public class BaseTest
    {
        protected IContainer AutofacContainer;
        
        [OneTimeSetUp]
        public virtual void SetUp()
        {
            var configuration = new ConfigurationBuilder().Build();

            var serviceCollection = new ServiceCollection();
            serviceCollection.AddAutoMapper(Assembly.GetExecutingAssembly(), Assembly.GetAssembly(typeof(MappingProfile)));
            serviceCollection.RegisterDataContexts(configuration);
            serviceCollection.RegisterConfiguration(configuration);

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
            builder.RegisterInstance(DbContextOptionsBuilderFactoryMock.GetDbContextOptionsBuilderFactory()).As<IDbContextOptionsBuilderFactory>();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
        }
    }
}
