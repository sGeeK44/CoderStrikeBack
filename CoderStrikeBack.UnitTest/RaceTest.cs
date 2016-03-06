
using System.Collections.Generic;
using Moq;
using NUnit.Framework;

namespace CoderStrikeBack.UnitTest
{
    [TestFixture]
    public class RaceTest
    {
        [TestCase]
        public void Create_ValidInputArg_ShouldInitCheckpointList()
        {
            var checkPointList = new List<Checkpoint> { Checkpoint.CreateFromLine(0, "1 2") };
            var race = Race.Create(1, checkPointList);

            Assert.AreEqual(checkPointList, race.CheckpointList);
        }

        [TestCase]
        public void Create_ValidInputArg_ShouldInitLaps()
        {
            var checkPointList = TestKit.CreateCheckpointList(1);

            const int expectedLaps = 1;
            var race = Race.Create(expectedLaps, checkPointList);

            Assert.AreEqual(expectedLaps, race.Laps);
        }

        [TestCase]
        public void Create_ValidInputArg_ShouldInitPlayerPodList()
        {
            var checkPointList = TestKit.CreateCheckpointList(1);
            var race = Race.Create(1, checkPointList);

            Assert.AreEqual(2, race.PlayerPodList.Count);
        }

        [TestCase]
        public void Create_ValidInputArg_ShouldInitOpponentPodList()
        {
            var checkPointList = TestKit.CreateCheckpointList(1);
            var race = Race.Create(1, checkPointList);

            Assert.AreEqual(2, race.OpponentPodList.Count);
        }

        [TestCase]
        public void UpdateFirstPlayerPod_ValidInputLine_ShouldUpdatePodWithoutException()
        {
            var race = TestKit.CreateValidRaceWithOneLapsOneCheckPoint();

            race.UpdatePlayerPod(0, TestKit.CreateValidPodLine());
        }

        [TestCase]
        public void UpdateFirstOpponentPod_ValidInputLine_ShouldUpdatePodWithoutException()
        {
            var race = TestKit.CreateValidRaceWithOneLapsOneCheckPoint();

            race.UpdateOpponentPod(0, TestKit.CreateValidPodLine());
        }

        [TestCase]
        public void UpdateSecondPlayerPod_ValidInputLine_ShouldUpdatePodWithoutException()
        {
            var race = TestKit.CreateValidRaceWithOneLapsOneCheckPoint();

            race.UpdatePlayerPod(1, TestKit.CreateValidPodLine());
        }

        [TestCase]
        public void UpdateSecondOpponentPod_ValidInputLine_ShouldUpdatePodWithoutException()
        {
            var race = TestKit.CreateValidRaceWithOneLapsOneCheckPoint();

            race.UpdateOpponentPod(1, TestKit.CreateValidPodLine());
        }

        [TestCase]
        public void ComputeNextCommand_ArgPodNull_ShouldReturnNullCommand()
        {
            var checkpointPosition = new Point(1, 1);
            var race = Race.Create(1, new List<Checkpoint> { new Checkpoint { Position = checkpointPosition } });

            var result = race.ComputeNextCommand(null);

            Assert.IsNull(result);
        }

        [TestCase]
        [Ignore("NeedFix")]
        public void ComputeNextCommand_OneCheckpoint_TargetPositionShouldCheckpointPosition()
        {
            var checkpoint1 = new Checkpoint { Position = new Point(1, 1) };
            var checkpoint2 = new Checkpoint { Position = new Point(2, 2) };
            var race = Race.Create(1, new List<Checkpoint> { checkpoint1, checkpoint2 });
            var pod = new Mock<Pod>();
            pod.Setup(_ => _.NextCheckpoint).Returns(checkpoint2);
            pod.Setup(_ => _.CurrentRace).Returns(race);
            pod.Setup(_ => _.CurrentPosition).Returns(new Point(0,0));

            var result = race.ComputeNextCommand(pod.Object);

            Assert.AreEqual(checkpoint2.Position, result.TargetPosition);
        }
    }
}
