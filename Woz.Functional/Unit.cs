using System;

namespace Woz.Functional
{
    public struct Unit : IEquatable<Unit>, IComparable<Unit>, IComparable
    {
        public static Unit Value => new Unit();

        public int CompareTo(Unit other) => 0;

        public int CompareTo(object? obj) => obj is Unit ? 0 : -1;

        public bool Equals(Unit other) => true;
    }
}
