using System;
using LementPro.Server.SvcTemplate.Sdk.Models.BlockWork;
using NUnit.Framework;
using System.Collections;

namespace LementPro.Server.SvcTemplate.Test.TestSources
{
    public class BlockWorkServiceTestSource
    {
        private static BlockWorkAddModel GetBlockWork(Action<BlockWorkAddModel> lambda = null)
        {
            var model = new BlockWorkAddModel
            {
                Name = "A",
                Description= "B",
            };

            lambda?.Invoke(model);
            return model;
        }


        public static IEnumerable CreateBlockWorkSource
        {
            get
            {
                yield return new TestCaseData(GetBlockWork()).SetName("create BlockkWork A");
                yield return new TestCaseData(GetBlockWork(x=>x.Name = "C")).SetName("create BlockkWork B");
            }
        }
    }
}
