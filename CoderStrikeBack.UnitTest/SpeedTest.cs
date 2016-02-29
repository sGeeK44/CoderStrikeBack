
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
    }
}
