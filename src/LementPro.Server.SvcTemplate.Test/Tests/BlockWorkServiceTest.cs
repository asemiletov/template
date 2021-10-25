using Autofac;
using LementPro.Server.Common.Repository.Seed.Abstract;
using LementPro.Server.SvcTemplate.Common.Enums;
using LementPro.Server.SvcTemplate.Repository.Context;
using LementPro.Server.SvcTemplate.Sdk.Abstract;
using LementPro.Server.SvcTemplate.Sdk.Models.BlockWork;
using LementPro.Server.SvcTemplate.Service.Abstract;
using LementPro.Server.SvcTemplate.Test.Base;
using LementPro.Server.SvcTemplate.Test.TestSources;
using NUnit.Framework;

namespace LementPro.Server.SvcTemplate.Test.Tests
{
    [Category("BlockWork tests")]
    [TestFixture]
    public class BlockWorkServiceTest : BaseTest
    {
        private IBlockWorkService _blockWorkService;

        private DataDbContext _dbContext;

        [OneTimeSetUp]
        public void RunBeforeAllTests()
        {
            _dbContext = GetDbContext();

            var seedFactory = AutofacContainer.Resolve<IDatabaseSeedFactory>();
            seedFactory.GetTemplate("services_blockwork").Start();
            
            _blockWorkService = AutofacContainer.Resolve<IBlockWorkService>();
        }

        [Order(1)]
        //[ MaxTime(5000)]
        [Test(Description = "Create BlockWork"), TestCaseSource(typeof(BlockWorkServiceTestSource), nameof(BlockWorkServiceTestSource.CreateBlockWorkSource))]
        public void CreateBlockWorkSuccess(BlockWorkAddModel data)
        {
            
            var entityId = 0L;
            Assert.DoesNotThrowAsync((async () =>
            {
                entityId = await _blockWorkService.Add(data);
            }),"Failed to create new BlockWork");

            BlockWorkModel srcModel = null;
            Assert.DoesNotThrowAsync((async () =>
            {
                srcModel = await _blockWorkService.Get(entityId);
            }), "Failed to get created BlockWork");

            Assert.NotNull(srcModel);

            Assert.AreEqual(data.Name, srcModel.Name);
            Assert.AreEqual(data.Description, srcModel.Description);
            Assert.AreEqual(BlockWorkStatus.New, srcModel.Status);

        }
    }
}
