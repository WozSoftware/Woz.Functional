using System;
using System.Collections.Generic;
using System.Linq;
using static Woz.Functional.FreeFunctions;

namespace Woz.Functional.Monads
{
    public static class MaybeEnumerable
    {
        public static IEnumerable<T> ToEnumerable<T>(this Maybe<T> maybe)
        {
            if (maybe.HasValue)
            {
                yield return maybe.Value;
            }
        }

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
    }
}
