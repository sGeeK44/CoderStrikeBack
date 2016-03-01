
using System.Collections.Generic;
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

        [TestCase]
        public void Constructor_ArgList_ShouldInitializedCommandList()
        {
            var podCommandList = new PodCommandList(new List<PodCommand>());

            Assert.IsEmpty(podCommandList.CommandList);
        }
    }
}
