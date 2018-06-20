using System;
using System.Collections.Generic;
using System.Linq;
using static Woz.Functional.FreeFunctions;

namespace Woz.Functional.Monads
{
    public struct Maybe<T> 
    {
        private readonly (T, bool) _contents;

        public static implicit operator Maybe<T>(T value) => value.ToMaybe();

        public static Maybe<T> None => new Maybe<T>((default(T), false));

        internal Maybe((T, bool) contents) => _contents = contents;

        public bool HasValue => _contents.Item2;

        public T Value
        {
            get
            {
                if (!HasValue)
                {
                    throw new InvalidOperationException(
                        $"{nameof(Maybe<T>)} does not have a value.");
                }
                return _contents.Item1;
            }
        }
    }

    public static class Maybe 
    {
        #region Construction
        public static Maybe<T> ToSome<T>(this T value) => new Maybe<T>((value, true));
        public static Maybe<T> ToMaybe<T>(this T value) => value != null ? value.ToSome() : Maybe<T>.None;
        #endregion

        #region LINQ=Map/Flatmap
        public static Maybe<TR> SelectMany<T, TR>(this Maybe<T> maybe, Func<T, Maybe<TR>> selector)
            => maybe.HasValue ? selector(maybe.Value) : Maybe<TR>.None;

        public static Maybe<TR> SelectMany<T1, T2, TR>(
            this Maybe<T1> maybe, Func<T1, Maybe<T2>> selector, Func<T1, T2, TR> projection)
            => maybe.SelectMany(
                value1 => selector(value1).SelectMany(value2 => projection(value1, value2).ToMaybe()));

        public static Maybe<TR> Select<T, TR>(this Maybe<T> maybe, Func<T, TR> selector)
            => maybe.SelectMany(x => selector(x).ToMaybe());

        public static Maybe<T> Where<T>(this Maybe<T> maybe, Func<T, bool> predicate)
            => maybe.SelectMany(value => predicate(value) ? value.ToSome() : Maybe<T>.None);
        #endregion

        #region Utility
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

        #region IEnumerable
        public static Maybe<T> MaybeMin<T>(this IEnumerable<T> enumerable)
            => enumerable.MaybeMin(Identity);

        public static Maybe<TResult> MaybeMin<T, TResult>(
            this IEnumerable<T> enumerable, Func<T, TResult> selector)
        {
            var cached = enumerable as ICollection<T> ?? enumerable.ToArray();
            return cached.Any() ? cached.Select(selector).Min().ToMaybe() : Maybe<TResult>.None;
        }

        public static Maybe<T> MaybeMax<T>(this IEnumerable<T> enumerable)
            => enumerable.MaybeMax(Identity);

        public static Maybe<TResult> MaybeMax<T, TResult>(
            this IEnumerable<T> enumerable, Func<T, TResult> selector)
        {
            var cached = enumerable as ICollection<T> ?? enumerable.ToArray();
            return cached.Any() ? cached.Select(selector).Max().ToMaybe() : Maybe<TResult>.None;
        }
        #endregion

        #region IDictionary
        public static Maybe<TValue> MaybeFind<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key) 
            => dictionary.TryGetValue(key, out TValue value) ? value.ToMaybe() : Maybe<TValue>.None;
        #endregion
    }
}
