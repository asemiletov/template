using LementPro.Server.SvcTemplate.Repository.Context;
using LementPro.Server.SvcTemplate.Service.Context.ConnectionString;
using LementPro.Server.SvcTemplate.Service.Context.Factory;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace LementPro.Server.SvcTemplate.Test.Mock
{
    /// <summary>
    /// Class for creating ContextFactory mock
    /// </summary>
    public static class DbContextOptionsBuilderFactoryMock
    {
        /// <summary>
        /// Create IDbContextOptionsBuilderFactory mock.
        /// </summary>
        /// <returns>Mocked IDbContextOptionsBuilderFactory instance</returns>
        public static IDbContextOptionsBuilderFactory GetDbContextOptionsBuilderFactory()
        {
            var optionsBuilderMock = new Mock<IDbContextOptionsBuilderFactory>();
            optionsBuilderMock.Setup(x => x.Create<DataDbContext>(It.IsAny<DbContextOptionsBuilder>()))
                .Returns((DbContextOptionsBuilder builder) =>
            {
                var optionsBuilder = builder ?? new DbContextOptionsBuilder<DataDbContext>();
                optionsBuilder.UseInMemoryDatabase(typeof(DataDbContext).Name);
                return optionsBuilder;
            });

            return optionsBuilderMock.Object;
        }
    }
}
