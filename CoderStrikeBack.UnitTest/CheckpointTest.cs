
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

        [TestCase]
        public void IsReach_LineOutsideCircle_ShouldReturnFalse()
        {
            var checkpoint = new Checkpoint();

            Assert.IsFalse(checkpoint.IsReach(new Vector(401, 401, -401, 401)));
        }

        [TestCase]
        public void IsReach_LineCutSegmentInside_ShouldReturnTrue()
        {
            var checkpoint = new Checkpoint();

            Assert.IsTrue(checkpoint.IsReach(new Vector(401, 200, -401, 200)));
        }

        [TestCase]
        public void IsReach_LineCutSegmentInOutside_ShouldReturnFalse()
        {
            var checkpoint = new Checkpoint();

            Assert.IsFalse(checkpoint.IsReach(new Vector(410, 200, 401, 200)));
        }

        [TestCase]
        public void IsReach_LineCutSegmentTotalyInside_ShouldReturnTrue()
        {
            var checkpoint = new Checkpoint();

            Assert.IsTrue(checkpoint.IsReach(new Vector(390, 200, -390, 200)));
        }

        [TestCase]
        public void IsReach_LineCutOriginSegmentInside_ShouldReturnTrue()
        {
            var checkpoint = new Checkpoint();

            Assert.IsTrue(checkpoint.IsReach(new Vector(390, 200, -401, 200)));
        }

        [TestCase]
        public void IsReach_LineCutTargetSegmentInside_ShouldReturnTrue()
        {
            var checkpoint = new Checkpoint();

            Assert.IsTrue(checkpoint.IsReach(new Vector(401, 200, -390, 200)));
        }
    }
}
