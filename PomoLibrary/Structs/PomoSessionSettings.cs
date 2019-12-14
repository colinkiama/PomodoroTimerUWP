using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PomoLibrary.Structs
{
    public struct PomoSessionSettings : IEquatable<PomoSessionSettings>
    {

        public PomoSessionLength WorkSessionLength { get; set; }
        public PomoSessionLength BreakSessionLength { get; set; }
        public PomoSessionLength LongBreakSessionLength { get; set; }

        public bool Equals(PomoSessionSettings other)
        {
            return this.WorkSessionLength == other.WorkSessionLength
                   && this.BreakSessionLength == other.BreakSessionLength
                   && this.LongBreakSessionLength == other.LongBreakSessionLength;
        }

        public override int GetHashCode()
        {
            int hash = 13;
            hash = (hash * 7) + WorkSessionLength.GetHashCode();
            hash = (hash * 7) + BreakSessionLength.GetHashCode();
            hash = (hash * 7) + LongBreakSessionLength.GetHashCode();
            return hash;
        }

        public static bool operator ==(PomoSessionSettings left, PomoSessionSettings right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(PomoSessionSettings left, PomoSessionSettings right)
        {
            return !(left == right);
        }

        public override bool Equals(object obj)
        {
            bool isEqual = false;
            if (obj is PomoSessionSettings otherSettings)
            {
                isEqual = this.Equals(otherSettings);
            }

            return isEqual;
        }
    }
}
