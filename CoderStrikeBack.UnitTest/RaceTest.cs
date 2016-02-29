﻿
using System.Collections.Generic;
using Moq;
using NUnit.Framework;

namespace CoderStrikeBack.UnitTest
{
    [TestFixture]
    public class RaceTest
    {
        [TestCase]
        public void Create_ValidInputArg_ShouldInitializedRace()
        {
            var checkPointList = new List<Checkpoint> { Checkpoint.CreateFromLine(0, "1 2") };
            const int laps = 1;
            var race = Race.Create(1, checkPointList);

            Assert.AreEqual(checkPointList, race.CheckpointList);
            Assert.AreEqual(laps, race.Laps);
            Assert.IsEmpty(race.PlayerPod);
            Assert.IsEmpty(race.OpponentPod);
        }

        [TestCase]
        public void UpdatePlayerPod_NewPod_PlayerPodCollectionShouldContainsNewItem()
        {
            var race = Race.Create(1, null);
            var playerPod1 = new Mock<Pod>();
            var playerPod2 = new Mock<Pod>();
            var newPlayerPodState = new List<Pod> { playerPod1.Object, playerPod2.Object };

            race.UpdatePlayerPod(newPlayerPodState);

            Assert.Contains(playerPod1.Object, race.PlayerPod);
            Assert.Contains(playerPod2.Object, race.PlayerPod);
        }

        [TestCase]
        public void UpdateOpponentPod_NewPod_OpponentPodCollectionShouldContainsNewItem()
        {
            var race = Race.Create(1, null);
            var opponentPod1 = new Mock<Pod>();
            var opponentPod2 = new Mock<Pod>();
            var newOpponentPodState = new List<Pod> { opponentPod1.Object, opponentPod2.Object };

            race.UpdateOpponentPod(newOpponentPodState);

            Assert.Contains(opponentPod1.Object, race.OpponentPod);
            Assert.Contains(opponentPod2.Object, race.OpponentPod);
        }

        [TestCase]
        public void ComputeNextCommands_NullCheckpointList_ShouldReturnNullCommand()
        {
            var race = Race.Create(1, null);

            Assert.IsNull(race.ComputeNextCommands());
        }

        [TestCase]
        public void ComputeNextCommands_EmptyCheckpointList_ShouldReturnNullCommand()
        {
            var race = Race.Create(1, new List<Checkpoint>());

            Assert.IsNull(race.ComputeNextCommands());
        }

        [TestCase]
        public void ComputeNextCommands_OneCheckpointTwoPlayerPod_ShouldReturnNullCommand()
        {
            var race = Race.Create(1, new List<Checkpoint> { new Checkpoint { Position = new Point(1, 1) } });
            race.UpdatePlayerPod(new List<Pod> { new Pod() });

            var result = race.ComputeNextCommands();
            
            Assert.AreEqual(1, result.CommandList.Count);
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
        public void ComputeNextCommand_OneCheckpoint_TargetPositionShouldCheckpointPosition()
        {
            var checkpointPosition = new Point(1, 1);
            var race = Race.Create(1, new List<Checkpoint> { new Checkpoint { Position = checkpointPosition } });

            var result = race.ComputeNextCommand(new Pod());

            Assert.AreEqual(checkpointPosition, result.TargetPosition);
        }
    }
}
