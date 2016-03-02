using System;
using System.Collections.Generic;
using System.Linq;

namespace CoderStrikeBack
{
    #region Program

    class Program
    {
        public const int CountPlayerPod = 2;
        public const int CountOpponentPod = 2;

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
        public Race GetRace()
        {
            var laps = GetLaps();
            var checkpointList = GetCheckpointList();
            return Race.Create(laps, checkpointList);
        }

        public void UpdateRace(Race race)
        {
            UpdatePlayerPodList(race);
            UpdateOpponentPodList(race);
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

        private static void UpdateOpponentPodList(Race race)
        {
            for (var i = 0; i < Program.CountOpponentPod; i++)
            {
                race.UpdateOpponentPod(i, Console.ReadLine());
            }
        }

        private static void UpdatePlayerPodList(Race race)
        {
            for (var i = 0; i < Program.CountPlayerPod; i++)
            {
                race.UpdatePlayerPod(i, Console.ReadLine());
            }
        }
    }

    #endregion

    #region PodCommand

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

    #region Race

    public class Race
    {
        #region Constructors

        private Race() { }

        #endregion

        #region Porperties

        public int Laps { get; set; }

        public IList<Checkpoint> CheckpointList { get; set; }

        public List<Pod> PlayerPodList { get; set; }

        public List<Pod> OpponentPodList { get; set; }

        #endregion

        #region Pod Commands

        public PodCommandList ComputeNextCommands()
        {
            return new PodCommandList(PlayerPodList.Select(ComputeNextCommand).ToList());
        }

        public PodCommand ComputeNextCommand(Pod podToMove)
        {
            if (podToMove == null) return null;

            return podToMove.ComputeNextCommand();
        }

        #endregion

        #region Refresh game state

        public void UpdatePlayerPod(int index, string line)
        {
            PlayerPodList[index].Update(line);
        }

        public void UpdateOpponentPod(int index, string line)
        {
            OpponentPodList[index].Update(line);
        }

        #endregion

        #region Services

        public Checkpoint GetFirstCheckPoint()
        {
            return CheckpointList.First();
        }

        public Checkpoint GetCheckpoint(int nextCheckPointId)
        {
            return CheckpointList[nextCheckPointId];
        }

        public Checkpoint GetLastCheckPoint()
        {
            return CheckpointList.Last();
        }

        public Checkpoint GetNextCheckpoint(Checkpoint currentTarget)
        {
            var currentIndex = CheckpointList.IndexOf(currentTarget);
            return CheckpointList[currentIndex + 1];
        }

        #endregion

        #region Factory methods

        public static Race Create(int laps, IList<Checkpoint> checkpoints)
        {
            if (checkpoints == null || checkpoints.Count == 0) throw new ArgumentException("checkpoints");

            var result = new Race
            {
                Laps = laps,
                CheckpointList = checkpoints,
                PlayerPodList = new List<Pod>(),
                OpponentPodList = new List<Pod>()
            };

            for (var i = 0; i < Program.CountPlayerPod; i++)
            {
                result.PlayerPodList.Add(Pod.Create(result));
            }

            for (var i = 0; i < Program.CountOpponentPod; i++)
            {
                result.OpponentPodList.Add(Pod.Create(result));
            }

            return result;
        }

        #endregion
    }

    #endregion

    #region Checkpoint

    public class Checkpoint
    {
        #region Properties

        public int Index { get; set; }

        public Point Position { get; set; }

        public int Radius { get { return 600; } }

        #endregion

        #region Services

        public bool IsReach(Pod playerPod)
        {
            if (playerPod == null) return false;

            var deltaX = Position.X - playerPod.CurrentPosition.X;
            var deltaXSquared = deltaX * deltaX;
            var deltaY = Position.Y - playerPod.CurrentPosition.Y;
            var deltaYSquared = deltaY * deltaY;
            var sumRadii = Radius;
            var sumRadiiSquared = sumRadii * sumRadii;

            return (deltaXSquared + deltaYSquared) <= sumRadiiSquared;
        }

        #endregion

        #region Factory methods

        public static Checkpoint CreateFromLine(int index, string p)
        {
            return new Checkpoint
            {
                Index = index,
                Position = Point.CreateFromLine(p)
            };
        }

        #endregion

        internal bool IsLast(Race race)
        {
            return race.GetLastCheckPoint() == this;
        }
    }

    #endregion

    #region Pod

    public class Pod
    {
        private int _nextCheckpointId;

        #region Constantes

        private const int POWER_MAX = 200;
        private const int MAX_SPEED_ANGLE_CELCIUS = 18;
        private const char INPUT_SEPARATOR = ' ';

        #endregion

        #region Constructor

        public Pod() { }

        public Pod(Race currentRace)
        {
            CurrentRace = currentRace;
        }

        #endregion

        #region Properties

        public Point CurrentPosition { get; set; }

        public Speed CurrentSpeed { get; set; }

        public int Angle { get; set; }

        public int Radius { get { return 400; } }

        public int Power { get; set; }

        public int NextCheckpointId
        {
            get { return _nextCheckpointId; }
            set
            {
                if (NextCheckpointIsReach(NextCheckpointId, value) && NextCheckpoint.IsLast(CurrentRace)) SetNewLap();
                _nextCheckpointId = value;
            }
        }

        public virtual Checkpoint NextCheckpoint { get { return CurrentRace.GetCheckpoint(NextCheckpointId); } }

        public Race CurrentRace { get; set; }

        public int CurrentLap { get; private set; }

        public bool HasLapsRemaining { get { return CurrentLap < CurrentRace.Laps; } }

        #endregion

        #region Refresh game state

        public void Update(string line)
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
            
            CurrentPosition = new Point(x, y);
            CurrentSpeed = new Speed(vx, vy);
            Angle = angle;
            NextCheckpointId = nextCheckPointId;
        }

        private static bool NextCheckpointIsReach(int actualNextCheckpoint, int newNextCheckPointId)
        {
            return actualNextCheckpoint != newNextCheckPointId;
        }

        #endregion

        #region Services

        public void SetNewLap()
        {
            CurrentLap++;
        }

        public bool IsLastCheckpointLap()
        {
            return CurrentRace.GetLastCheckPoint() == NextCheckpoint;
        }

        public bool HasReachTarget()
        {
            return NextCheckpoint.IsReach(this);
        }

        public PodCommand ComputeNextCommand()
        {
            var power = ComputePower();
            return new AcceleratePodCommand(NextCheckpoint.Position, power);
        }

        private int ComputePower()
        {
            return POWER_MAX;
        }

        #endregion

        #region Factory methods

        public static Pod Create(Race currentRace)
        {
            if (currentRace == null) throw new ArgumentNullException("currentRace");

            return new Pod(currentRace)
            {
                CurrentLap = 1
            };
        }

        #endregion
    }

    #endregion

    #region Geometrie

    #region Point

    public class Point : IEquatable<Point>
    {
        #region Constantes

        private const char INPUT_SEPARATOR = ' ';

        #endregion

        #region Constructors

        public Point(long x, long y)
        {
            X = x;
            Y = y;
        }

        #endregion

        #region Properties

        public long X { get; set; }

        public long Y { get; set; }

        #endregion

        #region Services

        public override string ToString()
        {
            return string.Format("{0} {1}", X, Y);
        }

        public override bool Equals(object other)
        {
            return Equals(other as Point);
        }

        public bool Equals(Point other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return X == other.X && Y == other.Y;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (X.GetHashCode() * 397) ^ Y.GetHashCode();
            }
        }

        public static bool operator ==(Point left, Point right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Point left, Point right)
        {
            return !Equals(left, right);
        }

        #endregion

        #region Factory methods

        public static Point CreateFromLine(string line)
        {
            if (string.IsNullOrEmpty(line)) throw new ArgumentNullException("line");

            var inputs = line.Split(INPUT_SEPARATOR);
            if (inputs == null || inputs.Length != 2) throw new ArgumentException(string.Format("La ligne d'entrée n'est pas correctement formatté. Attendu:x y. Reçu:{0}", line));

            var checkpointX = int.Parse(inputs[0]);
            var checkpointY = int.Parse(inputs[1]);
            return new Point(checkpointX, checkpointY);
        }


        #endregion
    }

    #endregion

    #region Angle

    public class Angle
    {
        #region Constructor

        private Angle(Vector v1, Vector v2)
        {
            V1 = v1;
            V2 = v2;
        }

        #endregion

        #region Properties

        public Vector V1 { get; private set; }

        public Vector V2 { get; private set; }

        public double Cos { get { return -V1.Scalar(V2) / (V1.Norm * V2.Norm); } }

        public int ValueInDegree { get { return (int)Math.Round(RadToDegree(ValueInRadian)); } }

        public double ValueInRadian { get { return Math.Acos(Cos); } }

        private static double RadToDegree(double rad)
        {
            return (rad * 180) / Math.PI;
        }

        #endregion

        #region Factory methods

        public static Angle CreateFromPoint(Point initialPoint, Point throughPoint, Point targetPoint)
        {
            var v1 = new Vector(initialPoint, throughPoint);
            var v2 = new Vector(throughPoint, targetPoint);

            return CreateFromVector(v1, v2);
        }

        public static Angle CreateFromVector(Vector v1, Vector v2)
        {
            if (v1.Norm <= 0) return null;
            if (v2.Norm <= 0) return null;

            return new Angle(v1, v2);
        }

        #endregion
    }

    #endregion

    #region Vector

    public class Vector : IEquatable<Vector>
    {
        #region Constructors

        public Vector(int x, int y)
        {
            Origin = new Point(0, 0);
            Target = new Point(x, y);
        }

        public Vector(Point origin, Point target)
        {
            if (origin == null) throw new ArgumentNullException("origin");
            if (target == null) throw new ArgumentNullException("target");

            Origin = origin;
            Target = target;
        }

        public Vector(Point origin, long x, long y)
        {
            if (origin == null) throw new ArgumentNullException("origin");

            Origin = origin;
            Target = new Point(Origin.X + x, Origin.Y + y);
        }

        #endregion

        #region Properties

        public long X
        {
            get { return Target.X - Origin.X; }
        }

        public long Y
        {
            get { return Target.Y - Origin.Y; }
        }

        public Point Origin { get; private set; }

        public Point Target { get; private set; }

        public double Norm
        {
            get { return Math.Sqrt(Math.Pow(X, 2) + Math.Pow(Y, 2)); }
        }

        #endregion

        #region Services

        public override bool Equals(object other)
        {
            return Equals(other as Vector);
        }

        public bool Equals(Vector other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(Origin, other.Origin) && Equals(Target, other.Target);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Origin != null ? Origin.GetHashCode() : 0) * 397) ^ (Target != null ? Target.GetHashCode() : 0);
            }
        }

        public static bool operator ==(Vector left, Vector right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Vector left, Vector right)
        {
            return !Equals(left, right);
        }

        public long Scalar(Vector other)
        {
            if (other == null) throw new ArgumentNullException("other");

            return X*other.X + Y*other.Y;
        }

        public Vector Sum(Vector ac)
        {
            return new Vector(Origin, X + ac.X, Y + ac.Y);
        }

        #endregion
    }

    #endregion

    #region Speed

    public class Speed
    {
        #region Fields

        private readonly Vector _value;

        #endregion

        #region Constructors

        public Speed(int speedX, int speedY)
        {
            _value = new Vector(speedX, speedY);
        }

        #endregion

        #region Properties

        public long X { get { return _value.X; } }

        public long Y { get { return _value.Y; } }

        public double AbsoluteValue { get { return _value.Norm; } }

        #endregion

        #region Services
        
        public override bool Equals(object other)
        {
            return Equals(other as Speed);
        }

        public bool Equals(Speed other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return X == other.X && Y == other.Y;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return _value.GetHashCode();
            }
        }

        public static bool operator ==(Speed left, Speed right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Speed left, Speed right)
        {
            return !Equals(left, right);
        }

        #endregion
    }

    #endregion

    #endregion

    #region Utils

    public class Helper
    {
        public static bool HasCollision(Checkpoint checkpoint, Pod playerPod)
        {
            if (checkpoint == null) return false;
            if (playerPod == null) return false;

            var deltaX = checkpoint.Position.X - playerPod.CurrentPosition.X;
            var deltaXSquared = deltaX * deltaX;
            var deltaY = checkpoint.Position.Y - playerPod.CurrentPosition.Y;
            var deltaYSquared = deltaY * deltaY;

            // Calculate the sum of the radii, then square it
            var sumRadii = checkpoint.Radius + playerPod.Radius;
            var sumRadiiSquared = sumRadii * sumRadii;

            return (deltaXSquared + deltaYSquared) <= sumRadiiSquared;
        }
    }

    #endregion
}
