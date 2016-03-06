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

        [TestCase]
        public void Equals_CompareToNull_ShouldBeNotEquals()
        {
            var firstVector = new Vector(1, 0);
            var secondVector = new Vector(0, 0);

            Assert.IsFalse(firstVector.Equals(secondVector));
        }

        [TestCase]
        public void Equals_XIsDifferent_ShouldBeNotEquals()
        {
            var firstVector = new Vector(1, 0);
            var secondVector = new Vector(0, 0);

            Assert.IsFalse(firstVector.Equals(secondVector));
        }

        [TestCase]
        public void Equals_YIsDifferent_ShouldBeNotEquals()
        {
            var firstVector = new Vector(0, 1);
            var secondVector = new Vector(0, 0);

            Assert.IsFalse(firstVector.Equals(secondVector));
        }

        [TestCase]
        public void Equals_SameCoordinate_ShouldBeEquals()
        {
            var firstVector = new Vector(1, 1);
            var secondVector = new Vector(1, 1);

            Assert.IsTrue(firstVector.Equals(secondVector));
        }

        [TestCase]
        public void Opposite_VecteurQuarterTopLeft_ShouldReturnRightResult()
        {
            var vector = new Vector(1, 1);
            var expected = new Vector(1, 1, 0, 0);

            Assert.AreEqual(expected, vector.Opposite());
        }

        [TestCase]
        public void Multiply_WithReal_ShouldReturnRightResult()
        {
            var vector = new Vector(10, (double)45);
            var expected = new Vector(30, (double)45);

            Assert.AreEqual(expected, vector * 3);
            Assert.AreEqual(expected, 3 * vector);
        }

        [TestCase(1, 0, 0)]
        [TestCase(1, 1, 45)]
        [TestCase(0, 1, 90)]
        [TestCase(-1, 1, 135)]
        [TestCase(-1, 0, 180)]
        [TestCase(-1, -1, -135)]
        [TestCase(0, -1, -90)]
        [TestCase(1, -1, -45)]
        public void GetAlphaCelcius_OnSpecifiedPostion_ShouldReturnRightResult(int x, int y, int expectedAlpha)
        {
            var vector = new Vector(x, y);

            Assert.AreEqual(expectedAlpha, vector.Alpha);
        }

        [TestCase(1, 0, 1, 0)]
        [TestCase(1, 90, 0, 1)]
        [TestCase(1, 180, -1, 0)]
        [TestCase(1, -90, 0, -1)]
        public void Constructor_FromNormAndAngle(long norm, int angle, int expectedX, int expectedY)
        {
            var vector = new Vector(norm, angle);
            var expectedVector = new Vector(expectedX, expectedY);

            Assert.AreEqual(expectedVector, vector);
        }

        [TestCase]
        public void Constructor_FromNormAndAngleE()
        {
            var vector = new Vector(Math.Sqrt(2), 45);
            var expectedVector = new Vector(1, 1);

            Assert.AreEqual(expectedVector, vector);
        }

        [TestCase(180, Math.PI)]
        [TestCase(360, 2*Math.PI)]
        [TestCase(90, Math.PI/2)]
        [TestCase(270, Math.PI/2 + Math.PI)]
        public void DegreeToRad_AngleInDegree_ShouldReturnAngleInRadian(int degree, double expectedRadian)
        {
            Assert.AreEqual(expectedRadian, Vector.DegreeToRad(degree));
        }

        [TestCase(Math.PI, 180)]
        [TestCase(2 * Math.PI, 360)]
        [TestCase(Math.PI / 2, 90)]
        [TestCase(Math.PI / 2 + Math.PI, 270)]
        public void RadToDegree_AngleInRadian_ShouldReturnAngleInDegree(double radian, int expectedDegree)
        {
            Assert.AreEqual(expectedDegree, Vector.RadToDegree(radian));
        }

        [TestCase]
        public void NextSpeed_Case1()
        {
            Assert.AreEqual(new Vector(96, 123), Pod.NextSpeed(new Vector(52, 66), 100));
        }

        [TestCase]
        public void NextSpeed_Case2()
        {
            Assert.AreEqual(new Vector(134, 170), Pod.NextSpeed(new Vector(96, 122), 100));
        }

        [TestCase]
        public void NextSpeed_Case3()
        {
            Assert.AreEqual(new Vector(166, 211), Pod.NextSpeed(new Vector(134, 170), 100));
        }

        private void AssertNorm(Vector vector, double expectedResult)
        {
            Assert.AreEqual(expectedResult, vector.Norm);
        }
    }
}
