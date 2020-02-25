using System;

namespace Amazerrr
{
    public class Position
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Position()
        {
        }

        public Position(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override string ToString()
        {
            return $"{X},{Y}";
        }

        public int ToKey()
        {
            return X + Y * 1_000;
        }

        public Position Clone()
        {
            return new Position(X, Y);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            Position other = (Position)obj;
            return (X == other.X) && (Y == other.Y);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }
    }
}
