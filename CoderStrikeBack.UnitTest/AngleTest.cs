using NUnit.Framework;

namespace CoderStrikeBack.UnitTest
{
    [TestFixture]
    public class AngleTest
    {
        [TestCase]
        public void Create_SameThreePoint_ShouldReturnNull()
        {
            var initialPoint = new Point(1, 1);
            var throughPoint = new Point(1, 1);
            var targetPoint = new Point(1, 1);

            var angle = Angle.CreateFromPoint(initialPoint, throughPoint, targetPoint);

            Assert.AreEqual(null, angle);
        }
        
        [TestCase]
        public void Create_DifferentInitAndSameThroughAndTarget_ShouldReturnNull()
        {
            var initialPoint = new Point(0, 0);
            var throughPoint = new Point(1, 1);
            var targetPoint = new Point(1, 1);

            var angle = Angle.CreateFromPoint(initialPoint, throughPoint, targetPoint);

            Assert.AreEqual(null, angle);
        }

        [TestCase]
        public void Create_SameInitAndThroughDifferentTarget_ShouldReturnNull()
        {
            var initialPoint = new Point(0, 0);
            var throughPoint = new Point(0, 0);
            var targetPoint = new Point(1, 1);

            var angle = Angle.CreateFromPoint(initialPoint, throughPoint, targetPoint);

            Assert.AreEqual(null, angle);
        }

        [TestCase(0, 0, 0)]
        [TestCase(1, 0, 45)]
        [TestCase(1, 1, 90)]
        [TestCase(1, 2, 135)]
        [TestCase(0, 2, 180)]
        [TestCase(-1, 2, 135)]
        [TestCase(-1, 1, 90)]
        [TestCase(-1, 0, 45)]
        public void Create_ThreeDifferentPoint_ShouldReturnRightAngle(int x, int y, int expectedAngle)
        {
            var initialPoint = new Point(0, 0);
            var throughPoint = new Point(0, 1);
            var targetPoint = new Point(x, y);

            var angle = Angle.CreateFromPoint(initialPoint, throughPoint, targetPoint);

            Assert.AreEqual(expectedAngle, angle.ValueInDegree);
        }

        [TestCase(1, 1, 1, 2, 180)]
        public void Create_FourDifferentPoint_ShouldReturnRightAngle(int x1, int y1, int x2, int y2, int expectedAngle)
        {
            var a = new Point(0, 0);
            var b = new Point(0, 1);
            var c = new Point(x1, y1);
            var d = new Point(x2, y2);
            var ab = new Vector(a, b);
            var cd = new Vector(c, d);

            var angle = Angle.CreateFromVector(ab, cd);

            Assert.AreEqual(expectedAngle, angle.ValueInDegree);
        }

        [TestCase]
        public void Create_ThreeDifferentPoint_ShouldReturnRightAngle()
        {
            var initialPoint = new Point(2, -3);
            var throughPoint = new Point(3, 1);
            var targetPoint = new Point(-1, 4);

            var angle = Angle.CreateFromPoint(initialPoint, throughPoint, targetPoint);

            Assert.AreEqual(113, angle.ValueInDegree);
        }
    }
}
