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

                consoleBuilder.WriteNextRound(new RoundGameOutput());
            }
        }
    }

    #endregion

    #region IGameBuilder

    public interface IGameBuilder
    {
        GlobalInput GetGlobalInput();
        RoundGameInput GetRoundInput();
        void WriteNextRound(RoundGameOutput output);
    }

    public class ConsoleBuilder : IGameBuilder
    {
        private const int COUNT_PLAYER_POD = 2;
        private const int COUNT_OPPONENT_POD = 2;

        public GlobalInput GetGlobalInput()
        {
            return new GlobalInput
            {
                Laps = GetLaps(),
                CheckpointList = GetCheckpointList()
            };
        }

        private static IList<Checkpoint> GetCheckpointList()
        {
            var checkpointCount = int.Parse(Console.ReadLine());
            var checkpointList = new List<Checkpoint>();
            for (var i = 0; i < checkpointCount; i++)
            {
                checkpointList.Add(Checkpoint.CreateFromLine(checkpointCount, Console.ReadLine()));
            }

            return checkpointList;
        }

        private static int GetLaps()
        {
            return int.Parse(Console.ReadLine());
        }

        public RoundGameInput GetRoundInput()
        {
            return new RoundGameInput
            {
                PlayerPod = GetPlayerPodList(),
                OppenentPod = GetOpponentPodList()
            };
        }

        public void WriteNextRound(RoundGameOutput output)
        {
            output.ExecuteCommand();
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
        public IList<Checkpoint> CheckpointList { get; set; }
    }

    public class RoundGameInput
    {
        public List<Pod> PlayerPod { get; set; }
        public List<Pod> OppenentPod { get; set; }
    }
    
    public class RoundGameOutput
    {
        public RoundGameOutput()
        {
            PlayerPod = new List<PodCommandBase>();
        }

        public List<PodCommandBase> PlayerPod { get; set; }

        public void ExecuteCommand()
        {
            foreach (var podCommandBase in PlayerPod)
            {
                podCommandBase.ExecuteCommand();
            }
        }
    }

    #endregion

    #region PodCommand

    public abstract class PodCommandBase
    {
        protected PodCommandBase(Point targetPosition)
        {
            if (targetPosition == null) throw new ArgumentNullException("targetPosition");

            TargetPosition = targetPosition;
        }
        public Point TargetPosition { get; set; }

        public void ExecuteCommand()
        {
            Console.WriteLine(Command);
        }

        public virtual string Command { get { return TargetPosition.ToString(); } }
    }

    public class AcceleratePodCommand : PodCommandBase
    {
        public AcceleratePodCommand(Point targetPosition, int power) : base(targetPosition)
        {
            Power = power;
        }

        public int Power { get; set; }

        public override string Command
        {
            get { return string.Format("{0} {1}", base.Command, Power); }
        }
    }

    public class ShieldPodCommand : PodCommandBase
    {
        public ShieldPodCommand(Point targetPosition) : base(targetPosition) { }

        public override string Command
        {
            get { return string.Format("{0} SHIELD", base.Command); }
        }
    }

    #endregion

    #region Game object

    public class Checkpoint
    {
        private Checkpoint() { }

        public static Checkpoint CreateFromLine(int index, string p)
        {
            return new Checkpoint
            {
                Index = index,
                Position = Point.CreateFromLine(p)
            };
        }

        public int Index { get; set; }

        public Point Position { get; set; }
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

        public override string ToString()
        {
            return string.Format("{0} {1}", X, Y);
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
        private const int POWER_MAX = 200;
        private const int ANGLE_CELCIUS_MAX = 18;
        private const int POD_RADIUS = 400;
        private const char INPUT_SEPARATOR = ' ';

        private Pod() { }

        public Point CurrentPosition { get; set; }
        public Speed CurrentSpeed { get; set; }
        public int Angle { get; set; }
        public int NextCheckPointId { get; set; }
        public int Radius { get { return POD_RADIUS; } }

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
