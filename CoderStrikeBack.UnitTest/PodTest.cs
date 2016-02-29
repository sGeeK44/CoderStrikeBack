
using System;
using NUnit.Framework;

namespace CoderStrikeBack.UnitTest
{
    [TestFixture]
    public class PodTest
    {
        [TestCase]
        public void CreateFromLine_NullInputLine_ShouldThrowException()
        {
            Assert.Throws<ArgumentNullException>(() => Pod.CreateFromLine(null));
        }

        [TestCase]
        public void CreateFromLine_EmptyInputLine_ShouldThrowException()
        {
            Assert.Throws<ArgumentNullException>(() => Pod.CreateFromLine(string.Empty));
        }

        [TestCase]
        public void CreateFromLine_InvalidInputLine_ShouldThrowException()
        {
            Assert.Throws<ArgumentException>(() => Pod.CreateFromLine("ddd"));
        }

        [TestCase(0, 0, 0, 0, 0, 0)]
        [TestCase(1, 1, 1, 1, 1, 1)]
        public void CreateFromLine_ValidInputLine_ShouldReturnInitializedPod(int x, int y, int vx, int vy, int angle, int nextChackPointId)
        {
            var pod = Pod.CreateFromLine(string.Format("{0} {1} {2} {3} {4} {5}", x, y, vx, vy, angle, nextChackPointId));
            Assert.AreEqual(x, pod.CurrentPosition.X);
            Assert.AreEqual(y, pod.CurrentPosition.Y);
            Assert.AreEqual(vx, pod.CurrentSpeed.X);
            Assert.AreEqual(vy, pod.CurrentSpeed.Y);
            Assert.AreEqual(angle, pod.Angle);
            Assert.AreEqual(nextChackPointId, pod.NextCheckPointId);
        }
    }
}
