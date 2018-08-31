using System;
using static Woz.Functional.FreeFunctions;

namespace Woz.Functional.Monads
{
    public delegate T Reader<in TEnv, out T>(TEnv env);

    public static class Reader
    {
        #region Construction
        public static Reader<TEnv, T> ToReader<TEnv, T>(this T value) => environment => value;
        public static Reader<TEnv, T> ToReader<TEnv, T>(this Func<TEnv, T> func) => new Reader<TEnv, T>(func);

        public static Reader<TEnv, Unit> Unit<TEnv>(Unit unit) => unit.ToReader<TEnv, Unit>();
        #endregion

        #region LINQ=Map/Flatmap
        public static Reader<TEnv, TR> SelectMany<TEnv, T, TR>(
            this Reader<TEnv, T> reader, Func<T, Reader<TEnv, TR>> selector)
            => environment => selector(reader(environment))(environment);

        public static Reader<TEnv, TR> SelectMany<TEnv, T1, T2, TR>(
            this Reader<TEnv, T1> reader, Func<T1, Reader<TEnv, T2>> selector, Func<T1, T2, TR> projection)
            => reader.SelectMany(
                value1 => selector(value1).Select(value2 => projection(value1, value2)));

        public static Reader<TEnv, TR> Select<TEnv, T, TR>(this Reader<TEnv, T> reader, Func<T, TR> selector)
            => environment => selector(reader(environment));
        #endregion

        #region Kleisli
        public static Func<T1, Reader<TEnv, TR>> Into<TEnv, T1, T2, TR>(
            this Func<T1, Reader<TEnv, T2>> f, Func<T2, Reader<TEnv, TR>> g)
            => value => f(value).SelectMany(g);

        public static Func<T1, Reader<TEnv, TR>> Into<TEnv, T1, T2, T3, TR>(
            this Func<T1, Reader<TEnv, T2>> f, Func<T2, Reader<TEnv, T3>> g, Func<T2, T3, TR> projection)
            => value => f(value).SelectMany(g, projection);
        #endregion

        #region Utility
        public static Func<Reader<TEnv, T>, Reader<TEnv, TR>> Lift<TEnv, T, TR>(Func<T, TR> function)
            => reader => reader.Select(function);

        public static Reader<TEnv, T> Flattern<TEnv, T>(
            this Reader<TEnv, Reader<TEnv, T>> readerReader) => readerReader.SelectMany(Identity);
        #endregion
    }
}
