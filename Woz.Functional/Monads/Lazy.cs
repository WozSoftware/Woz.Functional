using System;
using static Woz.Functional.FreeFunctions;

namespace Woz.Functional.Monads
{
    public static class Lazy
    {
        #region LINQ=Map/Flatmap
        public static Lazy<TR> SelectMany<T, TR>(this Lazy<T> lazy, Func<T, Lazy<TR>> selector)
            => new Lazy<TR>(() => selector(lazy.Value).Value);

        public static Lazy<TR> SelectMany<T1, T2, TR>(
            this Lazy<T1> lazy, Func<T1, Lazy<T2>> selector, Func<T1, T2, TR> projection)
            => new Lazy<TR>(() => selector(lazy.Value).Select(value => projection(lazy.Value, value)).Value);

        public static Lazy<TR> Select<T, TR>(this Lazy<T> lazy, Func<T, TR> selector)
            => new Lazy<TR>(() => selector(lazy.Value));
        #endregion

        #region Utility
        public static Lazy<T> Flattern<T>(this Lazy<Lazy<T>> lazyLazy) => lazyLazy.SelectMany(Identity);
        #endregion
    }
}
