using System;
using NUnit.Framework;

namespace CoderStrikeBack.UnitTest
{
    [TestFixture]
    public class VectorTest
    {
        [TestCase]
        public void Create_TwoPoint_NormShouldBeEqualZero()
        {
            var segment = new Vector(new Point(0, 0), new Point(0, 0));

            AssertNorm(segment, 0);
        }

        [TestCase(1, 0)]
        [TestCase(0, 1)]
        [TestCase(-1, 0)]
        [TestCase(0, -1)]
        public void Create_TwoPoint_NormShouldBeEqualOne(int x, int y)
        {
            var segment = new Vector(new Point(0, 0), new Point(x, y));

            AssertNorm(segment, 1);
        }

        [TestCase(1, 1)]
        [TestCase(-1, 1)]
        [TestCase(-1, -1)]
        [TestCase(1, -1)]
        public void Create_TwoPoint_NormShouldBeEqualSqrt_2(int x, int y)
        {
            var segment = new Vector(new Point(0, 0), new Point(x, y));

            AssertNorm(segment, Math.Sqrt(2));
        }

        [TestCase]
        public void GetX_ValidVector_ShouldReturnRightValue()
        {
            var a = new Point(2, -3);
            var b = new Point(3, 1);
            var ab = new Vector(a, b);

            Assert.AreEqual(1, ab.X);
        }

        [TestCase]
        public void GetY_ValidVector_ShouldReturnRightValue()
        {
            var a = new Point(2, -3);
            var b = new Point(3, 1);
            var ab = new Vector(a, b);

            Assert.AreEqual(4, ab.Y);
        }

        [TestCase]
        public void Scalar_ValidArg_ShouldReturnRightValue()
        {
            var a = new Point(2, -3);
            var b = new Point(3, 1);
            var c = new Point(-1, 4);
            var ab = new Vector(a, b);
            var ac = new Vector(a, c);

            Assert.AreEqual(25, ab.Scalar(ac));
        }

        private void AssertNorm(Vector segment, double expectedResult)
        {
            Assert.AreEqual(expectedResult, segment.Norm);
        }
    }
}
