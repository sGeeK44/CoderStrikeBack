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
            if (IsLastCheckpointIndex(currentIndex)) return GetFirstCheckPoint();
            return CheckpointList[currentIndex + 1];
        }

        private bool IsLastCheckpointIndex(int currentIndex)
        {
            return CheckpointList.Count - 1 == currentIndex;
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
        #region Fields

        private Circle _circle;

        #endregion
        #region Constructors

        public Checkpoint()
        {
            _circle = new Circle(new Point(), 400);
        }

        #endregion

        #region Properties

        public int Index { get; set; }

        public Point Position
        {
            get { return _circle.Center; }
            set { _circle.Center = value; }
        }

        public double Radius { get { return _circle.Radius; } }

        #endregion

        #region Services

        public bool IsReach(Pod playerPod)
        {
            if (playerPod == null) return false;

            return IsReach(new Vector(playerPod.CurrentPosition, playerPod.CurrentSpeed.Target));
        }

        public bool IsReach(Point position)
        {
            return _circle.HasCollision(position);
        }

        public bool IsReach(Vector AB)
        {
            return _circle.HasCollision(AB);
        }

        public bool IsLast(Race race)
        {
            return race.GetLastCheckPoint() == this;
        }

        #endregion

        #region Factory methods

        public static Checkpoint CreateFromLine(int index, string p)
        {
            return new Checkpoint
            {
                Index = index,
                _circle = new Circle(Point.CreateFromLine(p), 400)
            };
        }

        #endregion
    }

    #endregion

    #region Pod

    public class Pod
    {
        private int _nextCheckpointId;

        #region Constantes

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

        public virtual Point CurrentPosition { get; set; }

        public Vector CurrentSpeed { get; set; }

        public int AngleGetted { get; set; }

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

        public virtual Race CurrentRace { get; set; }

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
            CurrentSpeed = new Vector(vx, vy);
            AngleGetted = angle;
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

        public static Vector NextSpeed(Vector currentSpeed, int power)
        {
            var newSpeed = currentSpeed.Sum(new Vector(power, currentSpeed.Alpha));
            
            var radAlpha = Vector.DegreeToRad(newSpeed.Alpha);
            var x = (long)Math.Truncate(Math.Cos(radAlpha) * newSpeed.Norm * 0.85);
            var y = (long)Math.Truncate(Math.Sin(radAlpha) * newSpeed.Norm * 0.85);
            return new Vector(new Point(0, 0), new Point(x, y));
        }

        public PodCommand ComputeNextCommand()
        {
            bool? accelerate;
            var canReachNextPointWithoutPower = CanReachNextPointWithoutPower(CurrentSpeed, new Point(CurrentPosition), AngleGetted, out accelerate);

            if (canReachNextPointWithoutPower == null)
            {
                if (accelerate.GetValueOrDefault()) return new AcceleratePodCommand(NextCheckpoint.Position, 200);
                else return new AcceleratePodCommand(NextCheckpoint.Position, 0);
            }
            
            var nextNextCheckpoint = CurrentRace.GetNextCheckpoint(NextCheckpoint);
            var virageAngle = Angle.CreateFromPoint(CurrentPosition, NextCheckpoint.Position, nextNextCheckpoint.Position);
            
            var strategy = TurnStrategyFactory.Create(virageAngle);
            if (canReachNextPointWithoutPower.Norm < strategy.MaxSpeed) return new AcceleratePodCommand(NextCheckpoint.Position, 200);
            
            return new AcceleratePodCommand(nextNextCheckpoint.Position, 0);
        }

        private Vector CanReachNextPointWithoutPower(Vector currentSpeed, Point currentPosition, int currentAngle, out bool? accelerate)
        {
            if (currentSpeed.Norm == 0)
            {
                accelerate = true;
                return null;
            }
            
            currentPosition.Add(currentSpeed);
            var nextSpeed = NextSpeed(currentSpeed, 0);


            int nextAngle;
            var Bv = new Vector(currentPosition, currentSpeed.Target);
            var B = new Point(Bv.X, Bv.Y);
            var angleMax = Angle.CreateFromPoint(NextCheckpoint.Position, CurrentPosition, B);
            if (angleMax.ValueInDegree > 18) nextAngle = currentAngle - 18;
            else nextAngle = currentAngle - angleMax.ValueInDegree;


            var isReach = NextCheckpoint.IsReach(new Vector(currentPosition, currentSpeed.Target));
            Console.Error.WriteLine("ComputeArrivedSpeedWithPower. currentSpeed:{0}. currentPosition:{1}. isReach:{2}. Checkpoint:{3}.", currentSpeed, currentPosition, isReach, NextCheckpoint.Position);
            if (isReach)
            {
                if (nextAngle == 0)
                {
                    accelerate = null;
                    return nextSpeed;
                }
                else
                {
                    accelerate = false;
                    return null;
                }
            }
            
            return CanReachNextPointWithoutPower(nextSpeed, currentPosition, nextAngle, out accelerate);
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

    #region TurnStragegy

    public class TurnStrategyFactory
    {
        public static TurnStrategy Create(Angle virageAngle)
        {
            if (virageAngle.ValueInDegree > 135) return new LargeTurnStrategy();
            if (virageAngle.ValueInDegree > 90) return new MediumTurnStrategy();
            if (virageAngle.ValueInDegree > 45) return new SmallTurnStrategy();

            return new SpinTurnStrategy();
        }
    }

    public abstract class TurnStrategy
    {
        protected const int POWER_MAX = 200;
        protected const int MAX_SPEED_ANGLE_CELCIUS = 18;

        public abstract double MaxSpeed { get; }
    }

    public class LargeTurnStrategy  : TurnStrategy
    {
        public override double MaxSpeed { get { return 150; } }
    }

    public class MediumTurnStrategy : TurnStrategy
    {
        public override double MaxSpeed { get { return 100; } }
    }

    public class SmallTurnStrategy : TurnStrategy
    {
        public override double MaxSpeed { get { return 50; } }
    }

    public class SpinTurnStrategy : TurnStrategy
    {
        public override double MaxSpeed { get { return 10; } }
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

        public Point() { }

        public Point(long x, long y)
        {
            X = x;
            Y = y;
        }

        public Point(Point point)
        {
            if (point == null) throw new ArgumentNullException("point");

            X = point.X;
            Y = point.Y;
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

        public void Add(Vector currentSpeed)
        {
            if (currentSpeed == null) return;

            X += currentSpeed.X;
            Y += currentSpeed.Y;
        }


        #endregion
    }

    #endregion

    #region Circle

    public class Circle
    {
        private Point _center;
        private double _radius;
        public Point Center
        {
            get
            {
                return _center;
            }
            set
            {
                if (value == null) throw new ArgumentNullException();

                _center = value;
            }
        }

        public double Radius
        {
            get
            {
                return _radius;
            }
            set
            {
                if (value <= 0) throw new ArgumentException("Le rayon doit être supérieur à 0");

                _radius = value;
            }
        }

        public Circle(Point center, double radius)
        {
            Center = center;
            Radius = radius;
        }

        public bool HasCollision(Point position)
        {
            var deltaXSquared = Math.Pow(Center.X - position.X, 2);
            var deltaYSquared = Math.Pow(Center.Y - position.Y, 2);
            var radiusSquared = Math.Pow(Radius, 2);

            return (deltaXSquared + deltaYSquared) <= radiusSquared;
        }

        public bool HasCollision(Vector AB)
        {
            // A : Origine Segment
            // B : Target Segment
            // C : Circle center
            // I : Projection orthogonal de C sur la droite AB.

            var AC = new Vector(AB.Origin, Center);
            double numerateur = Math.Abs(AB.X * AC.Y - AB.Y * AC.X);   // norme du vecteur v

            double denominateur = AB.Norm;
            double CI = numerateur / denominateur;
            if (CI > Radius) return false;

            // A partir d'ici la droite du vecteur coupe le cercle
            // Reste à voir si le vecteur est dedans ou à l'extérieur.
            Vector BC = new Vector(AB.Target, Center);
            float pscal1 = AB.X * AC.X + AB.Y * AC.Y;  // produit scalaire
            float pscal2 = (-AB.X) * BC.X + (-AB.Y) * BC.Y;  // produit scalaire
            if (pscal1 >= 0 && pscal2 >= 0)
                return true;   // I entre A et B, ok.

            // dernière possibilité, A ou B dans le cercle
            if (HasCollision(AB.Origin))
                return true;
            if (HasCollision(AB.Target))
                return true;

            return false;
        }
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

        public Vector(int x, int y) : this (new Point(x, y)) { }

        public Vector(Point p)
        {
            Origin = new Point(0, 0);
            Target = p;
        }

        public Vector(double norm, double alpha)
        {
            Origin = new Point(0, 0);
            var radAlpha = DegreeToRad(alpha);
            var x = (long)Math.Round(Math.Cos(radAlpha) * norm);
            var y = (long)Math.Round(Math.Sin(radAlpha) * norm);
            Target = new Point(x, y);
        }

        public Vector(Point origin, Point target)
        {
            if (origin == null) throw new ArgumentNullException("origin");
            if (target == null) throw new ArgumentNullException("target");

            Origin = origin;
            Target = target;
        }

        public Vector(Vector left) : this(left.Origin, left.Target) { }

        public Vector(Point origin, long x, long y)
        {
            if (origin == null) throw new ArgumentNullException("origin");

            Origin = origin;
            Target = new Point(Origin.X + x, Origin.Y + y);
        }

        public Vector(int x1, int y1, int x2, int y2) : this(new Point(x1, y1), new Point(x2, y2)) { }

        #endregion

        #region Properties

        public long X
        {
            get { return Target.X - Origin.X; }
            set { Target.X = value + Origin.X; }
        }

        public long Y
        {
            get { return Target.Y - Origin.Y; }
            set { Target.Y = value + Origin.Y; }
        }

        public double Alpha
        {
            get
            {
                if (X > 0 && Y == 0) return 0;
                if (X == 0 && Y > 0) return 90;
                if (X == 0 && Y < 0) return -90;
                if (X < 0 && Y == 0) return 180;

                var result = X > 0 ? RadToDegree(GetAtanYX()) : RadToDegree(GetAtanXY()) + 90;
                if (Y < 0) result *= -1;
                return result;
            }
        }

        public Point Origin { get; private set; }

        public Point Target { get; private set; }

        public double Norm
        {
            get { return Math.Sqrt(Math.Pow(X, 2) + Math.Pow(Y, 2)); }
        }

        private double GetAtanYX()
        {
            return Math.Atan(Math.Abs((double)Y / X));
        }

        private double GetAtanXY()
        {
            return Math.Atan(Math.Abs((double)X / Y));
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

        public override string ToString()
        {
            return string.Format("Origine:{0}. Target:{1}.", Origin, Target);
        }

        public static Vector operator *(Vector left, double right)
        {
            return new Vector(left.Norm * right, left.Alpha);
        }

        public static Vector operator *(double left, Vector right)
        {
            return right * left;
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

        public Vector Opposite()
        {
            return new Vector(Target, Origin);
        }

        #endregion

        #region Utils

        public static double RadToDegree(double rad)
        {
            return (rad * 180) / Math.PI;
        }

        public static double DegreeToRad(double degree)
        {
            return degree * Math.PI / 180;
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
