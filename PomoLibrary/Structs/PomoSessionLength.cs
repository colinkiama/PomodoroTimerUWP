using PomoLibrary.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PomoLibrary.Structs
{
    public struct PomoSessionLength : IEquatable<PomoSessionLength>
    {
        public int Length { get; set; }
        public TimeUnit UnitOfLength { get; set; }

        public bool Equals(PomoSessionLength other)
        {
            return this.Length == other.Length && this.UnitOfLength == other.UnitOfLength;
        }

        public double TimeInMilliseconds => GetTimeInMilliseconds();
        public override bool Equals(object obj)
        {
            bool isEqual = false;
            if (obj is PomoSessionLength other)
            {
                isEqual = this.Equals(other);
            }
            return isEqual;
        }

        public override int GetHashCode()
        {
            int hash = 13;
            hash = (hash * 7) + Length.GetHashCode();
            hash = (hash * 7) + UnitOfLength.GetHashCode();

            return hash;
        }

        public static bool operator ==(PomoSessionLength left, PomoSessionLength right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(PomoSessionLength left, PomoSessionLength right)
        {
            return !(left == right);
        }

        private double GetTimeInMilliseconds()
        {
            return Length * (int)UnitOfLength;
        }
    }
}

