﻿
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

        [TestCase]
        public void Equals_CompareToNull_ShouldBeNotEquals()
        {
            var firstPoint = new Point(1, 0);
            var secondPoint = new Point(0, 0);

            Assert.IsFalse(firstPoint.Equals(secondPoint));
        }

        [TestCase]
        public void Equals_XIsDifferent_ShouldBeNotEquals()
        {
            var firstPoint = new Point(1, 0);
            var secondPoint = new Point(0, 0);

            Assert.IsFalse(firstPoint.Equals(secondPoint));
        }

        [TestCase]
        public void Equals_YIsDifferent_ShouldBeNotEquals()
        {
            var firstPoint = new Point(0, 1);
            var secondPoint = new Point(0, 0);

            Assert.IsFalse(firstPoint.Equals(secondPoint));
        }

        [TestCase]
        public void Equals_SameCoordinate_ShouldBeEquals()
        {
            var firstPoint = new Point(1, 1);
            var secondPoint = new Point(1, 1);

            Assert.IsTrue(firstPoint.Equals(secondPoint));
        }

        [TestCase(0, 0, 0, 0)]
        [TestCase(1, 1, 1, 1)]
        [TestCase(-1, -1, -1, -1)]
        public void Add_AtOriginValidSpeed_ShouldRightResult(int speedX, int speedY, int expectedX, int expectedY)
        {
            var point = new Point(0, 0);
            var expectedPoint = new Point(expectedX, expectedY);
            var speed = new Vector(speedX, speedY);

            point.Add(speed);

            Assert.AreEqual(expectedPoint, point);
        }

        [TestCase(0, 0, 1, 1)]
        [TestCase(1, 1, 2, 2)]
        [TestCase(-1, -1, 0, 0)]
        public void Add_AtSomePointValidSpeed_ShouldRightResult(int speedX, int speedY, int expectedX, int expectedY)
        {
            var point = new Point(1, 1);
            var expectedPoint = new Point(expectedX, expectedY);
            var speed = new Vector(speedX, speedY);

            point.Add(speed);

            Assert.AreEqual(expectedPoint, point);
        }
    }
}
