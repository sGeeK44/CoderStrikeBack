
using System.Collections.Generic;
using Moq;
using NUnit.Framework;

namespace CoderStrikeBack.UnitTest
{
    [TestFixture]
    public class PodCommandListTest
    {
        [TestCase]
        public void Constructor_DefaultConstructor_ShouldInitializedCommandList()
        {
            var podCommandList = new PodCommandList();

            Assert.IsEmpty(podCommandList.CommandList);
        }
    }
}
