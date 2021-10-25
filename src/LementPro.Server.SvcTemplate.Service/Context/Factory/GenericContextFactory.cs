using LementPro.Server.SvcTemplate.Service.Context.ConnectionString;
using Microsoft.EntityFrameworkCore;
using System;
using System.Reflection;

namespace LementPro.Server.SvcTemplate.Service.Context.Factory
{
    public interface IGenericContextFactory<out TContext> where TContext : DbContext
    {
        TContext CreateContext();
    }

    public class GenericContextFactory<TContext> : IGenericContextFactory<TContext> where TContext : DbContext
    {
        private readonly IDbContextOptionsBuilderFactory _optionsBuilder;
        private readonly Func<DbContextOptions, TContext> _factory;

        public GenericContextFactory(IDbContextOptionsBuilderFactory optionsBuilder, Func<DbContextOptions, TContext> factory)
        {
            _optionsBuilder = optionsBuilder;
            _factory = factory;
        }

        public TContext CreateContext() => _factory(_optionsBuilder.Create<TContext>().Options);
    }
}