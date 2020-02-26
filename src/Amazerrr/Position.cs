using System;

namespace Amazerrr
{
    public class Position : IEquatable<Position>
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
            return Equals(obj as Position);
        }

        public bool Equals(Position other)
        {
            if (ReferenceEquals(other, null))
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            if (this.GetType() != other.GetType())
            {
                return false;
            }

            return X == other.X &&
                   Y == other.Y;
        }

        public static bool operator ==(Position lhs, Position rhs)
        {
            if (ReferenceEquals(lhs, null))
            {
                if (ReferenceEquals(rhs, null))
                {
                    return true;
                }

                return false;
            }
            return lhs.Equals(rhs);
        }

        public static bool operator !=(Position left, Position right) => !(left == right);

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }
    }
}
