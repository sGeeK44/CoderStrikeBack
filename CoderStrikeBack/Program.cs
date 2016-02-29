using System;
using System.Collections.Generic;
using System.Linq;

namespace CoderStrikeBack
{
    #region Program

    class Program
    {
        static void Main()
        {
            var consoleBuilder = new ConsoleBuilder();
            var race = consoleBuilder.GetRace();

            // game loop
            while (true)
            {
                consoleBuilder.UpdateRace(race);
                var nextCommands = race.ComputeNextCommands();
                consoleBuilder.ExecuteCommands(nextCommands);
            }
        }
    }

    #endregion

    #region GameBuilder

    public class ConsoleBuilder
    {
        private const int COUNT_PLAYER_POD = 2;
        private const int COUNT_OPPONENT_POD = 2;

        public Race GetRace()
        {
            var laps = GetLaps();
            var checkpointList = GetCheckpointList();
            return Race.Create(laps, checkpointList);
        }

        public void UpdateRace(Race race)
        {
            var playerPod = GetPlayerPodList();
            race.UpdatePlayerPod(playerPod);

            var opponentPod = GetOpponentPodList();
            race.UpdateOpponentPod(opponentPod);

            race.UpdateState();
        }

        public void ExecuteCommands(PodCommandList nextCommands)
        {
            foreach (var podCommand in nextCommands.CommandList)
            {
                Console.WriteLine(podCommand.Command);
            }
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

    public class Race
    {
        private Race() { }

        public int Laps { get; set; }
        public IList<Checkpoint> CheckpointList { get; set; }
        public List<Pod> PlayerPodList { get; set; }
        public List<Pod> OpponentPodList { get; set; }
        public Checkpoint CurrentTarget { get; private set; }
        public bool HasLapsRemaining { get { return CurrentLaps < Laps; } }
        public bool IsLastCheckpointLap { get { return CheckpointList.Last() == CurrentTarget; } }
        public int CurrentLaps { get; private set; }

        public PodCommandList ComputeNextCommands()
        {
            if (CheckpointList == null || CheckpointList.Count == 0) return null;

            return new PodCommandList(PlayerPodList.Select(_ => ComputeNextCommand(_)).ToList());
        }

        public PodCommand ComputeNextCommand(Pod podToMove)
        {
            if (podToMove == null) return null;
            if (CurrentTarget == null) return new OriginMoveCommand();

            return new AcceleratePodCommand(CurrentTarget.Position, 100);
        }

        public void UpdatePlayerPod(List<Pod> playerPod)
        {
            PlayerPodList = playerPod;
        }

        public void UpdateOpponentPod(List<Pod> opponentPod)
        {
            OpponentPodList = opponentPod;
        }

        public static Race Create(int laps, IList<Checkpoint> checkpoints)
        {
            var result = new Race
            {
                Laps = laps,
                CheckpointList = checkpoints,
                CurrentLaps = 1,
                PlayerPodList = new List<Pod>(),
                OpponentPodList = new List<Pod>()
            };

            result.ComputeFirstTarget();

            return result;
        }

        private void ComputeFirstTarget()
        {
            CurrentTarget = CheckpointList != null ? CheckpointList.FirstOrDefault() : null;
        }

        public void UpdateState()
        {
            if (PlayerhasReachTarget()) ComputeNextCheckpoint();
        }

        private void ComputeNextCheckpoint()
        {
            if (!IsLastCheckpointLap) CompteNextTargetInSameLap();
            else if (HasLapsRemaining) SetNewLap();
            else CurrentTarget = null;
        }

        private void CompteNextTargetInSameLap()
        {
            CurrentTarget = CheckpointList.FirstOrDefault(_ => _.Index == CurrentTarget.Index + 1);
        }

        private void SetNewLap()
        {
            CurrentLaps++;
            ComputeFirstTarget();
        }

        private bool PlayerhasReachTarget()
        {
            foreach(var playerPod in PlayerPodList)
            {
                if (CurrentTarget.IsReach(playerPod)) return true;
            }

            return false;
        }
    }

    public class PodCommandList
    {
        private List<PodCommand> list;

        public PodCommandList()
        {
            CommandList = new List<PodCommand>();
        }

        public PodCommandList(List<PodCommand> list)
        {
            CommandList = list;
        }

        public List<PodCommand> CommandList { get; set; }
    }

    #endregion

    #region PodCommand

    public abstract class PodCommand
    {
        protected PodCommand(Point targetPosition)
        {
            if (targetPosition == null) throw new ArgumentNullException("targetPosition");

            TargetPosition = targetPosition;
        }
        public Point TargetPosition { get; set; }

        public virtual string Command { get { return TargetPosition.ToString(); } }
    }

    public class OriginMoveCommand : PodCommand
    {
        public OriginMoveCommand() : base(new Point(0, 0)) { }
    }

    public class AcceleratePodCommand : PodCommand
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

    public class ShieldPodCommand : PodCommand
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
        private const int RADIUS = 400;

        public Checkpoint() { }

        public static Checkpoint CreateFromLine(int index, string p)
        {
            return new Checkpoint
            {
                Index = index,
                Position = Point.CreateFromLine(p)
            };
        }

        internal bool IsReach(Pod playerPod)
        {
            if (playerPod == null) return false;
            return Helper.HasCollision(this, playerPod);
        }

        public int Index { get; set; }

        public Point Position { get; set; }
        public int Radius { get { return RADIUS; } }
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
        private const int RADIUS = 400;
        private const char INPUT_SEPARATOR = ' ';

        public Point CurrentPosition { get; set; }
        public Speed CurrentSpeed { get; set; }
        public int Angle { get; set; }
        public int NextCheckPointId { get; set; }
        public int Radius { get { return RADIUS; } }

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

    #region Utils

    public class Helper
    {
        public static bool HasCollision(Checkpoint checkpoint, Pod playerPod)
        {
            if (checkpoint == null) return false;
            if (playerPod == null) return false;
            return checkpoint.Position == playerPod.CurrentPosition;
        }
    }

    #endregion
}
