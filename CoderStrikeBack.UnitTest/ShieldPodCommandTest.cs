
using System;
using NUnit.Framework;

namespace CoderStrikeBack.UnitTest
{
    [TestFixture]
    public class ShieldPodCommandTest
    {
        [TestCase]
        public void Constructor_TargetArgumentNull_ShouldThrowException()
        {
            Assert.Throws<ArgumentNullException>(() => new ShieldPodCommand(null));
        }

        [TestCase]
        public void Constructor_ValidTargetArgument_ShouldInitializedTargetPosition()
        {
            var point = new Point(0, 0);

            var result = new ShieldPodCommand(point);

            Assert.AreEqual(point, result.TargetPosition);
        }

        [TestCase(0, 0)]
        public void Command_ValidTargetPoint_ShouldReturnShieldCommand(int x, int y)
        {
            var command = new ShieldPodCommand(new Point(x, y));

            Assert.AreEqual(string.Format("{0} {1} SHIELD", x, y), command.Command);
        }
    }
}
