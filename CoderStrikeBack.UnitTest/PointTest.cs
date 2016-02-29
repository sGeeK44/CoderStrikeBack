
using System;
using NUnit.Framework;

namespace CoderStrikeBack.UnitTest
{
    [TestFixture]
    public class PointTest
    {
        [TestCase(0, 0)]
        [TestCase(1, 1)]
        [TestCase(-1, -1)]
        public void Constructor_ValidInputArg_ShouldInitializedPoint(int x, int y)
        {
            var point = new Point(x, y);
            Assert.AreEqual(x, point.X);
            Assert.AreEqual(y, point.Y);
        }

        [TestCase]
        public void CreateFromLine_NullInputLine_ShouldThrowException()
        {
            Assert.Throws<ArgumentNullException>(() => Point.CreateFromLine(null));
        }

        [TestCase]
        public void CreateFromLine_EmptyInputLine_ShouldThrowException()
        {
            Assert.Throws<ArgumentNullException>(() => Point.CreateFromLine(string.Empty));
        }

        [TestCase]
        public void CreateFromLine_InvalidInputLine_ShouldThrowException()
        {
            Assert.Throws<ArgumentException>(() => Point.CreateFromLine("ddd"));
        }

        [TestCase(0, 0)]
        [TestCase(1, 1)]
        [TestCase(-1, -1)]
        public void CreateFromLine_ValidInputLine_ShouldReturnInitializedPoint(int x, int y)
        {
            var point = Point.CreateFromLine(string.Format("{0} {1}", x, y));
            Assert.AreEqual(x, point.X);
            Assert.AreEqual(y, point.Y);
        }

        [TestCase(0, 0)]
        [TestCase(1, 1)]
        [TestCase(-1, -1)]
        public void ToString_ValidPoint_ShouldReturnStringRepresentation(int x, int y)
        {
            var point = new Point(x, y);

            Assert.AreEqual(string.Format("{0} {1}", x, y), point.ToString());
        }
    }
}
