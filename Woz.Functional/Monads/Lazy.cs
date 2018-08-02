using System;
using static Woz.Functional.FreeFunctions;

namespace Woz.Functional.Monads
{
    public static class Lazy
    {
        #region Construction
        public static Lazy<T> ToLazy<T>(this T value) => new Lazy<T>(() => value);
        public static Lazy<T> ToLazy<T>(this Func<T> factory) => new Lazy<T>(factory);
        public static Lazy<Func<T, TR>> ToLazy<T, TR>(this Func<T, TR> func) => new Lazy<Func<T, TR>>(() => func);
        #endregion

        #region LINQ=Map/Flatmap
        public static Lazy<TR> SelectMany<T, TR>(this Lazy<T> lazy, Func<T, Lazy<TR>> selector)
            => new Lazy<TR>(() => selector(lazy.Value).Value);

        public static Lazy<TR> SelectMany<T1, T2, TR>(
            this Lazy<T1> lazy, Func<T1, Lazy<T2>> selector, Func<T1, T2, TR> projection)
            => lazy.SelectMany(
                value1 => selector(value1).Select(value2 => projection(value1, value2)));

        public static Lazy<TR> Select<T, TR>(this Lazy<T> lazy, Func<T, TR> selector)
            => new Lazy<TR>(() => selector(lazy.Value));
        #endregion

        #region Kleisli
        public static Func<T1, Lazy<TR>> Into<T1, T2, TR>(
            this Func<T1, Lazy<T2>> f, Func<T2, Lazy<TR>> g)
            => value => f(value).SelectMany(g);

        public static Func<T1, Lazy<TR>> Into<T1, T2, T3, TR>(
            this Func<T1, Lazy<T2>> f, Func<T2, Lazy<T3>> g, Func<T2, T3, TR> projection)
            => value => f(value).SelectMany(g, projection);
        #endregion

        #region Utility
        public static Func<Lazy<T>, Lazy<TR>> Lift<T, TR>(Func<T, TR> function)
            => lazy => lazy.Select(function);

        public static Lazy<TR> Apply<T, TR>(this Lazy<T> lazy, Lazy<Func<T, TR>> lazyFunction)
            => from value in lazy
               from function in lazyFunction
               select function(value);

        public static Lazy<T> Flattern<T>(this Lazy<Lazy<T>> lazyLazy) => lazyLazy.SelectMany(Identity);
        #endregion
    }
}
