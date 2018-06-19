using System;

namespace Woz.Functional
{
    public struct Unit : IEquatable<Unit>, IComparable<Unit>, IComparable
    {
        public static Unit Value => new Unit();

        public int CompareTo(Unit other) => 0;

        public int CompareTo(object obj)
        {
            var _ = (Unit)obj;
            return 0;
        }

        public bool Equals(Unit other) => true;
    }
}
