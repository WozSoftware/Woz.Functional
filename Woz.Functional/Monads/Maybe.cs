using System;
using System.Collections.Generic;
using System.Linq;
using static Woz.Functional.FreeFunctions;

namespace Woz.Functional.Monads
{
    public abstract class Maybe<T> 
    {
        public static Maybe<T> Some(T value) => new MaybeSome(value);
        public static Maybe<T> None => new MaybeNone();

        private Maybe() { } // Hide

        public abstract bool HasValue { get; }

        public abstract T Value { get; }

        internal sealed class MaybeSome : Maybe<T>
        {
            internal MaybeSome(T value) => Value = value;

            public override bool HasValue => true;

            public override T Value { get; }
        }

        internal sealed class MaybeNone : Maybe<T>
        {
            public override bool HasValue => false;

            public override T Value => 
                throw new InvalidOperationException($"{nameof(MaybeNone)} does not have a value.");
        }
    }

    public static class Maybe 
    {
        #region Construction
        public static Maybe<T> ToSome<T>(this T value) => Maybe<T>.Some(value);
        public static Maybe<T> ToMaybe<T>(this T value) => value != null ? value.ToSome() : Maybe<T>.None;
        #endregion

        #region LINQ=Map/Flatmap
        public static Maybe<TR> SelectMany<T, TR>(this Maybe<T> maybe, Func<T, Maybe<TR>> selector)
            => maybe.HasValue ? selector(maybe.Value) : Maybe<TR>.None;

        public static Maybe<TR> SelectMany<T1, T2, TR>(
            this Maybe<T1> maybe, Func<T1, Maybe<T2>> selector, Func<T1, T2, TR> projection)
            => maybe.SelectMany(
                value1 => selector(value1).Select(value2 => projection(value1, value2)));

        public static Maybe<TR> Select<T, TR>(this Maybe<T> maybe, Func<T, TR> selector)
            => maybe.SelectMany(x => selector(x).ToMaybe());

        public static Maybe<T> Where<T>(this Maybe<T> maybe, Func<T, bool> predicate)
            => maybe.SelectMany(value => predicate(value) ? value.ToSome() : Maybe<T>.None);
        #endregion

        #region Kleisli
        public static Func<T1, Maybe<TR>> Into<T1, T2, TR>(
            this Func<T1, Maybe<T2>> f, Func<T2, Maybe<TR>> g)
            => value => f(value).SelectMany(g);

        public static Func<T1, Maybe<TR>> Into<T1, T2, T3, TR>(
            this Func<T1, Maybe<T2>> f, Func<T2, Maybe<T3>> g, Func<T2, T3, TR> projection)
            => value => f(value).SelectMany(g, projection);
        #endregion

        #region Utility
        public static Func<Maybe<T>, Maybe<TR>> Lift<T, TR>(this Func<T, TR> function)
            => maybe => maybe.Select(function);

        public static Maybe<TR> Apply<T, TR>(this Maybe<T> maybe, Maybe<Func<T, TR>> maybeFunction)
            => from value in maybe
               from function in maybeFunction
               select function(value);

        public static Maybe<T> Flattern<T>(this Maybe<Maybe<T>> maybeMaybe) => maybeMaybe.SelectMany(Identity);

        public static TR Match<T, TR>(this Maybe<T> maybe, Func<T, TR> someFunc, Func<TR> noneFunc)
            => maybe.HasValue ? someFunc(maybe.Value) : noneFunc();

        public static Maybe<T> Tee<T>(this Maybe<T> maybe, Action<T> action)
        {
            if (maybe.HasValue)
            {
                action(maybe.Value);
            }
            return maybe;
        }
        #endregion

        #region Recovery
        public static Maybe<T> Recover<T>(this Maybe<T> maybe, T defaultValue)
            => maybe.Recover(() => defaultValue);

        public static Maybe<T> Recover<T>(this Maybe<T> maybe, Func<T> defaultFactory)
            => maybe.HasValue ? maybe : defaultFactory().ToMaybe();
        #endregion
    }
}
