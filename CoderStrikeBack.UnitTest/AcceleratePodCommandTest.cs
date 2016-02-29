
using System;
using NUnit.Framework;

namespace CoderStrikeBack.UnitTest
{
    [TestFixture]
    public class AcceleratePodCommandTest
    {
        [TestCase]
        public void Constructor_TargetArgumentNull_ShouldThrowException()
        {
            Assert.Throws<ArgumentNullException>(() => new AcceleratePodCommand(null, 0));
        }

        [TestCase(0, 0, 0)]
        [TestCase(1, 1, 1)]
        public void Constructor_ValidTargetArgument_ShouldInitializedInnerProperties(int x, int y, int p)
        {
            var point = new Point(x, y);

            var result = new AcceleratePodCommand(point, p);

            Assert.AreEqual(point, result.TargetPosition);
            Assert.AreEqual(p, result.Power);
        }

        [TestCase(0, 0, 0)]
        [TestCase(1, 1, 1)]
        public void Command_ValidTargetPoint_ShouldReturnShieldCommand(int x, int y, int p)
        {
            var command = new AcceleratePodCommand(new Point(x, y), p);

            Assert.AreEqual(string.Format("{0} {1} {2}", x, y, p), command.Command);
        }
    }
}
