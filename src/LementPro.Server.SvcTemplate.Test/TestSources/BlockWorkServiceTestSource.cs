using LementPro.Server.SvcTemplate.Sdk.Models.BlockWork;
using NUnit.Framework;
using System.Collections;

namespace LementPro.Server.SvcTemplate.Test.TestSources
{
    public class BlockWorkServiceTestSource
    {
        public static IEnumerable CreateBlockWorkSource
        {
            get
            {
                yield return new TestCaseData(new BlockWorkAddModel() {Name = "A", Description = "B"}).SetName("create BlockkWork A");
                yield return new TestCaseData(new BlockWorkAddModel() {Name = "B", Description = "C"}).SetName("create BlockkWork B");
            }
        }
    }
}
