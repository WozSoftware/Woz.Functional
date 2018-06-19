using System;

namespace Woz.Functional.Monads
{
    public struct Maybe<T> 
    {
        private readonly (T, bool) _contents;

        public static Maybe<T> None => new Maybe<T>((default(T), false));

        //internal Maybe(T value) => _contents = (value, true);

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
        public static Maybe<T> ToMaybe<T>(this T value) => value != null ? new Maybe<T>((value, true)) : Maybe<T>.None;
        #endregion

        #region LINQ=Map/Flatmap
        public static Maybe<TR> SelectMany<T, TR>(
             this Maybe<T> maybe, Func<T, Maybe<TR>> selector)
            => maybe.HasValue
                ? selector(maybe.Value)
                : Maybe<TR>.None;

        public static Maybe<TR> SelectMany<T1, T2, TR>(
            this Maybe<T1> maybe,
            Func<T1, Maybe<T2>> selector,
            Func<T1, T2, TR> projection)
            => maybe.HasValue
                ? selector(maybe.Value).Select(value => projection(maybe.Value, value))
                : Maybe<TR>.None;

        public static Maybe<TR> Select<T, TR>(this Maybe<T> maybe, Func<T, TR> selector)
            => maybe.SelectMany(x => selector(x).ToMaybe());

        public static Maybe<T> Where<T>(
            this Maybe<T> maybe, Func<T, bool> predicate)
            => maybe.SelectMany(value => predicate(value) ? value.ToSome() : Maybe<T>.None);
        #endregion

        #region Recovery
        public static Maybe<T> Recover<T>(
            this Maybe<T> maybe, T defaultValue)
            => maybe.HasValue ? maybe : defaultValue.ToMaybe();

        public static Maybe<T> Recover<T>(
            this Maybe<T> maybe, Func<T> defaultFactory)
            => maybe.HasValue ? maybe : defaultFactory().ToMaybe();
        #endregion


    }
}
