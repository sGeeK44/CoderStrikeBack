
using System;
using NUnit.Framework;

namespace CoderStrikeBack.UnitTest
{
    [TestFixture]
    public class PodTest
    {
        [TestCase]
        public void Create_NullRace_ShouldThrowException()
        {
            Assert.Throws<ArgumentNullException>(() => Pod.Create(null));
        }

        [TestCase]
        public void Update_EmptyInputLine_ShouldThrowException()
        {
            var pod = new Pod(null);

            Assert.Throws<ArgumentNullException>(() => pod.Update(string.Empty));
        }

        [TestCase]
        public void Update_InvalidInputLine_ShouldThrowException()
        {
            var pod = new Pod(null);

            Assert.Throws<ArgumentException>(() => pod.Update("ddd"));
        }

        [TestCase(0, 0)]
        [TestCase(1, 1)]
        public void Update_ValidInputLine_ShouldUpdateCurrentPosition(int x, int y)
        {
            var pod = new Pod(null);
            var expectedPosition = new Point(x, y);

            pod.Update(TestKit.CreatePodLine(x, y, 0, 0, 0, 0));
            Assert.AreEqual(expectedPosition, pod.CurrentPosition);
        }

        [TestCase(0, 0)]
        [TestCase(1, 1)]
        public void Update_ValidInputLine_ShouldUpdateCurrentSpeed(int vx, int vy)
        {
            var pod = new Pod(null);
            var expectedSpeed = new Speed(vx, vy);

            pod.Update(TestKit.CreatePodLine(0, 0, vx, vy, 0, 0));

            Assert.AreEqual(expectedSpeed, pod.CurrentSpeed);
        }

        [TestCase(0)]
        [TestCase(1)]
        public void Update_ValidInputLine_ShouldUpdateAngle(int angle)
        {
            var pod = new Pod(null);

            pod.Update(TestKit.CreatePodLine(0, 0, 0, 0, angle, 0));

            Assert.AreEqual(angle, pod.Angle);
        }

        [TestCase(0)]
        [TestCase(1)]
        public void Update_ValidInputLine_ShouldUpdateNextCheckpoint(int nextChackPointId)
        {
            var pod = new Pod(null);

            pod.Update(TestKit.CreatePodLine(0, 0, 0, 0, 0, nextChackPointId));

            Assert.AreEqual(nextChackPointId, pod.NextCheckPointId);
        }
    }
}
