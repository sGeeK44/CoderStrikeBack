
using NUnit.Framework;

namespace CoderStrikeBack.UnitTest
{
    [TestFixture]
    public class SpeedTest
    {
        [TestCase(0, 0)]
        [TestCase(1, 1)]
        [TestCase(-1, -1)]
        public void Constructor_ValidInputArg_ShouldInitializedSpeed(int vx, int vy)
        {
            var speed = new Speed(vx, vy);
            Assert.AreEqual(vx, speed.X);
            Assert.AreEqual(vy, speed.Y);
        }

        [TestCase]
        public void Equals_CompareToNull_ShouldBeNotEquals()
        {
            var firstSpeed = new Speed(1, 0);
            var secondSpeed = new Speed(0, 0);

            Assert.IsFalse(firstSpeed.Equals(secondSpeed));
        }

        [TestCase]
        public void Equals_XIsDifferent_ShouldBeNotEquals()
        {
            var firstSpeed = new Speed(1, 0);
            var secondSpeed = new Speed(0, 0);

            Assert.IsFalse(firstSpeed.Equals(secondSpeed));
        }

        [TestCase]
        public void Equals_YIsDifferent_ShouldBeNotEquals()
        {
            var firstSpeed = new Speed(0, 1);
            var secondSpeed = new Speed(0, 0);

            Assert.IsFalse(firstSpeed.Equals(secondSpeed));
        }

        [TestCase]
        public void Equals_SameCoordinate_ShouldBeEquals()
        {
            var firstSpeed = new Speed(1, 1);
            var secondSpeed = new Speed(1, 1);

            Assert.IsTrue(firstSpeed.Equals(secondSpeed));
        }

        [TestCase]
        [Ignore("")]
        public void X()
        {
            var firstSpeed = new Speed(407, -22);

            Assert.AreEqual(0, firstSpeed.AbsoluteValue);
        }

        [TestCase]
        [Ignore("")]
        public void X2()
        {
            var force = new Vector(6043, 2764, 4171, 7467).Norm * 50;
            
            var expectedSpeed = new Speed(-15, 39).AbsoluteValue;

            Assert.AreEqual(expectedSpeed, force);
        }
    }
}
