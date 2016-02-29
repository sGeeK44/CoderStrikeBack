using System;
using System.Collections.Generic;

namespace CoderStrikeBack
{
    #region Program

    class Program
    {
        static void Main()
        {
            var consoleBuilder = new ConsoleBuilder();

            var globalInput = consoleBuilder.GetGlobalInput();

            // game loop
            while (true)
            {
                var roundInput = consoleBuilder.GetRoundInput();

                consoleBuilder.WriteNextRound();
            }
        }
    }

    #endregion

    #region IGameBuilder

    public interface IGameBuilder
    {
        GlobalInput GetGlobalInput();
        RoundGameInput GetRoundInput();
        void WriteNextRound();
    }

    public class ConsoleBuilder : IGameBuilder
    {
        private const int COUNT_PLAYER_POD = 2;
        private const int COUNT_OPPONENT_POD = 2;

        public GlobalInput GetGlobalInput()
        {
            var laps = int.Parse(Console.ReadLine());
            var checkpointCount = int.Parse(Console.ReadLine());
            var checkpointList = new List<Point>();
            for (var i = 0; i < checkpointCount; i++)
            {
                checkpointList.Add(Point.CreateFromLine(Console.ReadLine()));
            }

            return new GlobalInput
            {
                Laps = laps,
                CheckpointList = checkpointList
            };
        }

        public RoundGameInput GetRoundInput()
        {
            return new RoundGameInput
            {
                PlayerPod = GetPlayerPodList(),
                OppenentPod = GetOpponentPodList()
            };
        }

        public void WriteNextRound()
        {

        }

        private static List<Pod> GetOpponentPodList()
        {
            var opponentPodList = new List<Pod>();
            for (var i = 0; i < COUNT_OPPONENT_POD; i++)
            {
                opponentPodList.Add(CreatePodFromConsole());
            }
            return opponentPodList;
        }

        private static List<Pod> GetPlayerPodList()
        {
            var playerPodList = new List<Pod>();
            for (var i = 0; i < COUNT_PLAYER_POD; i++)
            {
                playerPodList.Add(CreatePodFromConsole());
            }
            return playerPodList;
        }

        private static Pod CreatePodFromConsole()
        {
            return Pod.CreateFromLine(Console.ReadLine());
        }
    }

    public class GlobalInput
    {
        public int Laps { get; set; }
        public IList<Point> CheckpointList { get; set; }
    }

    public class RoundGameInput
    {
        public List<Pod> PlayerPod { get; set; }
        public List<Pod> OppenentPod { get; set; }
    }

    #endregion

    #region Geometrie

    public class Point
    {
        private const char INPUT_SEPARATOR = ' ';

        public int X { get; set; }
        public int Y { get; set; }

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public static Point CreateFromLine(string line)
        {
            if (string.IsNullOrEmpty(line)) throw new ArgumentNullException("line");

            var inputs = line.Split(INPUT_SEPARATOR);
            if (inputs == null || inputs.Length != 2) throw new ArgumentException(string.Format("La ligne d'entrée n'est pas correctement formatté. Attendu:x y. Reçu:{0}", line));

            var checkpointX = int.Parse(inputs[0]);
            var checkpointY = int.Parse(inputs[1]);
            return new Point(checkpointX, checkpointY);
        }
    }

    public class Speed
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Speed(int speedX, int speedY)
        {
            X = speedX;
            Y = speedY;
        }
    }

    #endregion

    #region Pod

    public class Pod
    {
        private const char INPUT_SEPARATOR = ' ';

        private Pod() { }

        public Point CurrentPosition { get; set; }
        public Speed CurrentSpeed { get; set; }
        public int Angle { get; set; }
        public int NextCheckPointId { get; set; }

        public static Pod CreateFromLine(string line)
        {
            if (string.IsNullOrEmpty(line)) throw new ArgumentNullException("line");

            var inputs = line.Split(INPUT_SEPARATOR);
            if (inputs == null || inputs.Length != 6) throw new ArgumentException(string.Format("La ligne d'entrée n'est pas correctement formatté. Attendu:x y vx vy angle nextCheckPointId. Reçu:{0}", line));

            var x = int.Parse(inputs[0]);
            var y = int.Parse(inputs[1]);
            var vx = int.Parse(inputs[2]);
            var vy = int.Parse(inputs[3]);
            var angle = int.Parse(inputs[4]);
            var nextCheckPointId = int.Parse(inputs[5]);

            return new Pod
            {
                CurrentPosition = new Point(x, y),
                CurrentSpeed = new Speed(vx, vy),
                Angle = angle,
                NextCheckPointId = nextCheckPointId
            };
        }
    }

    #endregion
}
