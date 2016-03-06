
using System;
using NUnit.Framework;

namespace CoderStrikeBack.UnitTest
{
    [TestFixture]
    public class CircleTest
    {
        [TestCase]
        public void Constructors_NullCenter_ShouldThrowException()
        {
            Assert.Throws<ArgumentNullException>(() => new Circle(null, 0));
        }

        [TestCase]
        public void Constructors_RadiusValueIsEqualThanZero_ShouldThrowException()
        {
            Assert.Throws<ArgumentException>(() => new Circle(new Point(), 0));
        }

        [TestCase]
        public void Constructors_RadiusValueIsLessThanZero_ShouldThrowException()
        {
            Assert.Throws<ArgumentException>(() => new Circle(new Point(), -1));
        }


        [TestCase(0, 0)]
        [TestCase(1, 1)]
        public void Constructors_ValidPosition_CenterShouldBeEqualToSpecifiedPosition(int x, int y)
        {
            var position = new Point(x, y);
            var result = new Circle(position, 1);

            Assert.AreEqual(position, result.Center);
        }

        [TestCase(1)]
        [TestCase(100)]
        public void Constructors_ValidRadius_RadiusShouldBeEqualToSpecifiedRadius(double radius)
        {
            var position = new Point(0, 0);
            var result = new Circle(new Point(), radius);
            
            Assert.AreEqual(radius, result.Radius);
        }

        [TestCase]
        public void HasCollision_TopLineOutsideCircle_ShouldReturnFalse()
        {
            var circle = new Circle(new Point(), 2);

            Assert.IsFalse(circle.HasCollision(new Vector(3, 3, -3, 3)));
        }

        [TestCase]
        public void HasCollision_BottomLineOutsideCircle_ShouldReturnFalse()
        {
            var circle = new Circle(new Point(), 2);

            Assert.IsFalse(circle.HasCollision(new Vector(3, -3, -3, -3)));
        }

        [TestCase]
        public void HasCollision_RightLineOutsideCircle_ShouldReturnFalse()
        {
            var circle = new Circle(new Point(), 2);

            Assert.IsFalse(circle.HasCollision(new Vector(3, 3, 3, -3)));
        }

        [TestCase]
        public void HasCollision_LeftLineOutsideCircle_ShouldReturnFalse()
        {
            var circle = new Circle(new Point(), 2);

            Assert.IsFalse(circle.HasCollision(new Vector(-3, 3, -3, -3)));
        }

        [TestCase]
        public void HasCollision_TopLineOnCircle_ShouldReturnFalse()
        {
            var circle = new Circle(new Point(), 2);

            Assert.IsTrue(circle.HasCollision(new Vector(2, 2, -2, 2)));
        }

        [TestCase]
        public void HasCollision_BottomLineOnCircle_ShouldReturnFalse()
        {
            var circle = new Circle(new Point(), 2);

            Assert.IsTrue(circle.HasCollision(new Vector(2, -2, -2, -2)));
        }

        [TestCase]
        public void HasCollision_RightLineOnCircle_ShouldReturnFalse()
        {
            var circle = new Circle(new Point(), 2);

            Assert.IsTrue(circle.HasCollision(new Vector(2, 2, 2, -2)));
        }

        [TestCase]
        public void HasCollision_LeftLineOnCircle_ShouldReturnFalse()
        {
            var circle = new Circle(new Point(), 2);

            Assert.IsTrue(circle.HasCollision(new Vector(-2, 2, -2, -2)));
        }

        [TestCase]
        public void HasCollision_TopLineCutSegmentInside_ShouldReturnTrue()
        {
            var circle = new Circle(new Point(), 2);

            Assert.IsTrue(circle.HasCollision(new Vector(3, 1, -3, 1)));
        }

        [TestCase]
        public void HasCollision_BottomLineCutSegmentInside_ShouldReturnTrue()
        {
            var circle = new Circle(new Point(), 2);

            Assert.IsTrue(circle.HasCollision(new Vector(3, -1, -3, -1)));
        }

        [TestCase]
        public void HasCollision_LeftLineCutSegmentInside_ShouldReturnTrue()
        {
            var circle = new Circle(new Point(), 2);

            Assert.IsTrue(circle.HasCollision(new Vector(-1, 3, -1, -3)));
        }

        [TestCase]
        public void HasCollision_RightLineCutSegmentInside_ShouldReturnTrue()
        {
            var circle = new Circle(new Point(), 2);

            Assert.IsTrue(circle.HasCollision(new Vector(1, 3, 1, -3)));
        }

        [TestCase]
        public void HasCollision_LineCutSegmentOutside_ShouldReturnFalse()
        {
            var circle = new Circle(new Point(), 2);

            Assert.IsFalse(circle.HasCollision(new Vector(3, 1, 4, 1)));
        }

        [TestCase]
        public void HasCollision_LineCutSegmentTotalyInside_ShouldReturnTrue()
        {
            var circle = new Circle(new Point(), 2);

            Assert.IsTrue(circle.HasCollision(new Vector(1, 1, -1, 1)));
        }

        [TestCase]
        public void HasCollision_LineCutOriginSegmentInside_ShouldReturnTrue()
        {
            var circle = new Circle(new Point(), 2);

            Assert.IsTrue(circle.HasCollision(new Vector(1, 1, -3, 1)));
        }

        [TestCase]
        public void HasCollision_LineCutTargetSegmentInside_ShouldReturnTrue()
        {
            var circle = new Circle(new Point(), 2);

            Assert.IsTrue(circle.HasCollision(new Vector(3, 1, -1, 1)));
        }
    }
}
