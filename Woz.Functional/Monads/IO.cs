using System;
using static Woz.Functional.FreeFunctions;

namespace Woz.Functional.Monads
{
    public delegate T IO<out T>();

    public static class IO
    {
        #region Construction
        public static IO<T> ToIO<T>(this Func<T> func) => new IO<T>(func);
        public static IO<Func<T, TR>> ToIO<T, TR>(this Func<T, TR> func) => new IO<Func<T, TR>>(() => func);
        #endregion

        #region LINQ=Map/Flatmap
        public static IO<TR> SelectMany<T, TR>(this IO<T> io, Func<T, IO<TR>> selector)
            => selector(io());

        public static IO<TR> SelectMany<T1, T2, TR>(
            this IO<T1> io, Func<T1, IO<T2>> selector, Func<T1, T2, TR> projection)
            => io.SelectMany(
                value1 => selector(value1).Select(value2 => projection(value1, value2)));

        public static IO<TR> Select<T, TR>(this IO<T> io, Func<T, TR> selector)
            => () => selector(io());
        #endregion

        #region Kleisli
        public static Func<T1, IO<TR>> Into<T1, T2, TR>(
            this Func<T1, IO<T2>> f, Func<T2, IO<TR>> g)
            => value => f(value).SelectMany(g);

        public static Func<T1, IO<TR>> Into<T1, T2, T3, TR>(
            this Func<T1, IO<T2>> f, Func<T2, IO<T3>> g, Func<T2, T3, TR> projection)
            => value => f(value).SelectMany(g, projection);
        #endregion

        #region Utility
        public static Func<IO<T>, IO<TR>> Lift<T, TR>(Func<T, TR> function)
            => io => io.Select(function);

        public static IO<TR> Apply<T, TR>(this IO<T> io, IO<Func<T, TR>> ioFunc)
            => from value in io
               from function in ioFunc
               select function(value);

        public static IO<T> Flattern<T>(this IO<IO<T>> ioIo) => ioIo.SelectMany(Identity);
        #endregion

        #region Exception Wrapping
        public static Result<T, Exception> Run<T>(this IO<T> io) => Result.Try(new Func<T>(io));
        #endregion
    }
}
