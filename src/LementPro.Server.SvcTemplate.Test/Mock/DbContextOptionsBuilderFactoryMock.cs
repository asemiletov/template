using LementPro.Server.Common.EntityFramework.Factories.Abstract;
using LementPro.Server.SvcTemplate.Repository.Context;
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
        /// Create GetDbContextOptionsBuilderInMemoryFactory mock. InMemory.
        /// </summary>
        /// <returns>Mocked IDbContextOptionsBuilderFactory instance</returns>
        public static IDbContextOptionsBuilderFactory GetDbContextOptionsBuilderInMemoryFactory()
        {
            var optionsBuilderMock = new Mock<IDbContextOptionsBuilderFactory>();
            optionsBuilderMock.Setup(x => x.CreateNpgsql<DataDbContext>(It.IsAny<DbContextOptionsBuilder>()))
                .Returns((DbContextOptionsBuilder builder) =>
                {
                    var optionsBuilder = builder ?? new DbContextOptionsBuilder<DataDbContext>();
                    optionsBuilder.UseInMemoryDatabase(typeof(DataDbContext).Name);
                    return optionsBuilder;
                });

            return optionsBuilderMock.Object;
        }

        /// <summary>
        /// Create GetDbContextOptionsBuilderSqlLiteFactory mock. SqlLite.
        /// </summary>
        /// <returns>Mocked IDbContextOptionsBuilderFactory instance</returns>
        public static IDbContextOptionsBuilderFactory GetDbContextOptionsBuilderSqlLiteFactory()
        {
            var optionsBuilderMock = new Mock<IDbContextOptionsBuilderFactory>();
            optionsBuilderMock.Setup(x => x.CreateNpgsql<DataDbContext>(It.IsAny<DbContextOptionsBuilder>()))
                .Returns((DbContextOptionsBuilder builder) =>
                {
                    var optionsBuilder = builder ?? new DbContextOptionsBuilder<DataDbContext>();
                    builder.UseSqlite("Filename=SvcTemplate.db");
                    return optionsBuilder;
                });

            return optionsBuilderMock.Object;
        }
    }
}
