﻿using System.Collections.Generic;

namespace CoderStrikeBack.UnitTest
{
    public static class TestKit
    {
        public static string CreateValidPodLine()
        {
            return CreatePodLine(1, 1, 1, 1, 1, 0);
        }

        public static string CreatePodLine(int x, int y, int vx, int vy, int angle, int nextChackPointId)
        {
            return string.Format("{0} {1} {2} {3} {4} {5}", x, y, vx, vy, angle, nextChackPointId);
        }

        public static Race CreateValidRaceWithOneLapsOneCheckPoint()
        {
            var checkpointList = CreateCheckpointList(1);
            return Race.Create(1, checkpointList);
        }

        public static Race CreateValidRaceWithTwoLapsTwoCheckPoint()
        {
            var checkpointList = CreateCheckpointList(2);
            return Race.Create(2, checkpointList);
        }

        public static List<Checkpoint> CreateCheckpointList(int count)
        {
            var result = new List<Checkpoint>();
            for (var i = 0; i < count; i++)
            {
                result.Add(Checkpoint.CreateFromLine(i, string.Format("{0} {0}", i + 1)));
            }
            return result;
        }
    }
}
