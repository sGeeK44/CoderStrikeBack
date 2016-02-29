
using System;
using NUnit.Framework;

namespace CoderStrikeBack.UnitTest
{
    [TestFixture]
    public class CheckpointTest
    {
        [TestCase]
        public void CreateFromLine_LineArgumentNull_ShouldThrowException()
        {
            Assert.Throws<ArgumentNullException>(() => Checkpoint.CreateFromLine(0, null));
        }

        [TestCase(0, 0, 0)]
        [TestCase(1, 1, 1)]
        public void CreateFromLine_ValidArgument_ShouldInitializedInnerProperties(int index, int x, int y)
        {
            var result = Checkpoint.CreateFromLine(index, string.Format("{0} {1}", x, y));

            Assert.AreEqual(index, result.Index);
            Assert.AreEqual(x, result.Position.X);
            Assert.AreEqual(y, result.Position.Y);
        }
    }
}
