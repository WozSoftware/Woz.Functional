using System;

namespace Woz.Functional.Monads
{
    public abstract class Result<T, TE>
    {
        public static implicit operator Result<T, TE>(T value) => new ResultValue(value);

        public static Result<T, TE> Create(T value) => new ResultValue(value);
        public static Result<T, TE> Raise(TE error) => new ResultError(error);

        public abstract bool HasValue { get; }

        public abstract T Value { get; }
        public abstract TE Error { get; }

        internal class ResultValue : Result<T, TE>
        {
            internal ResultValue(T value) => Value = value;

            public override bool HasValue => true;

            public override T Value { get; }

            public override TE Error
                => throw new InvalidOperationException($"{nameof(Result<T, TE>)} does not have an error.");
        }

        internal class ResultError : Result<T, TE>
        {
            internal ResultError(TE error) => Error = error;

            public override bool HasValue => false;

            public override T Value
                => throw new InvalidOperationException($"{nameof(Result<T, TE>)} does not have a value.");

            public override TE Error { get; }
        }
    }

    public static class Result
    {
        #region LINQ=Map/Flatmap
        public static Result<TR, TE> SelectMany<T, TR, TE>(
            this Result<T, TE> result, Func<T, Result<TR, TE>> selector)
            => result.HasValue ? selector(result.Value) : Result<TR, TE>.Raise(result.Error);

        public static Result<TR, TE> SelectMany<T1, T2, TR, TE>(
            this Result<T1, TE> result, Func<T1, Result<T2, TE>> selector, Func<T1, T2, TR> projection)
            => result.SelectMany(
                value1 => selector(value1).SelectMany(
                    value2 => Result<TR, TE>.Create(projection(value1, value2))));

        public static Result<TR, TE> Select<T, TR, TE>(this Result<T, TE> result, Func<T, TR> selector)
            => result.SelectMany(x => Result<TR, TE>.Create(selector(x)));
        #endregion
    }
}
