using System;
using NUnit.Framework;

namespace CoderStrikeBack.UnitTest
{
    [TestFixture]
    public class VectorTest
    {
        [TestCase]
        public void Create_OnePoint_OriginShouldBeZeroTarget()
        {
            var vector = new Vector(1, 1);

            Assert.AreEqual(new Point(0, 0), vector.Origin);
        }

        [TestCase]
        public void Create_OnePoint_TargetShouldBeSpecifiedParam()
        {
            var vector = new Vector(1, 1);

            Assert.AreEqual(new Point(1, 1), vector.Target);
        }

        [TestCase]
        public void Create_TwoPoint_NormShouldBeEqualZero()
        {
            var vector = new Vector(new Point(0, 0), new Point(0, 0));

            AssertNorm(vector, 0);
        }

        [TestCase(1, 0)]
        [TestCase(0, 1)]
        [TestCase(-1, 0)]
        [TestCase(0, -1)]
        public void Create_TwoPoint_NormShouldBeEqualOne(int x, int y)
        {
            var vector = new Vector(new Point(0, 0), new Point(x, y));

            AssertNorm(vector, 1);
        }

        [TestCase(1, 1)]
        [TestCase(-1, 1)]
        [TestCase(-1, -1)]
        [TestCase(1, -1)]
        public void Create_TwoPoint_NormShouldBeEqualSqrt_2(int x, int y)
        {
            var vector = new Vector(new Point(0, 0), new Point(x, y));

            AssertNorm(vector, Math.Sqrt(2));
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

        [TestCase]
        public void Sum_ValidArgFromOrigin_ShouldReturnRightValue()
        {
            var a = new Point(0, 0);
            var b = new Point(0, 1);
            var c = new Point(1, 0);
            var ab = new Vector(a, b);
            var ac = new Vector(a, c);
            var expectedResult = new Vector(new Point(0, 0), new Point(1, 1));

            Assert.AreEqual(expectedResult, ab.Sum(ac));
        }

        [TestCase]
        public void Sum_ValidArg_ShouldReturnRightValue()
        {
            var a = new Point(1, 1);
            var b = new Point(2, 2);
            var c = new Point(-1, 0);
            var d = new Point(-3, 0);
            var ab = new Vector(a, b);
            var cd = new Vector(c, d);
            var expectedResult = new Vector(new Point(1, 1), new Point(0, 2));

            Assert.AreEqual(expectedResult, ab.Sum(cd));
        }

        private void AssertNorm(Vector vector, double expectedResult)
        {
            Assert.AreEqual(expectedResult, vector.Norm);
        }
    }
}
