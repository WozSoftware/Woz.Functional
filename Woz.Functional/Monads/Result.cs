using System;
using static Woz.Functional.FreeFunctions;

namespace Woz.Functional.Monads
{
    public abstract class Result<T, TE>
    {
        public static implicit operator Result<T, TE>(T value) => new ResultValue(value);

        public static Result<T, TE> Create(T value) => new ResultValue(value);
        public static Result<T, TE> Raise(TE error) => new ResultError(error);

        private Result() { } // Hide

        public abstract bool HasValue { get; }

        public abstract T Value { get; }
        public abstract TE Error { get; }

        internal sealed class ResultValue : Result<T, TE>
        {
            internal ResultValue(T value) => Value = value;

            public override bool HasValue => true;

            public override T Value { get; }

            public override TE Error
                => throw new InvalidOperationException($"{nameof(Result<T, TE>)} does not have an error.");
        }

        internal sealed class ResultError : Result<T, TE>
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

        #region Kleisli
        public static Func<T1, Result<TR, TE>> Into<T1, T2, TR, TE>(
            this Func<T1, Result<T2, TE>> f, Func<T2, Result<TR, TE>> g)
            => value => f(value).SelectMany(g);

        public static Func<T1, Result<TR, TE>> Into<T1, T2, T3, TR, TE>(
            this Func<T1, Result<T2, TE>> f, Func<T2, Result<T3, TE>> g, Func<T2, T3, TR> projection)
            => value => f(value).SelectMany(g, projection);
        #endregion

        #region Utility
        public static Func<Result<T, TE>, Result<TR, TE>> Lift<T, TR, TE>(Func<T, TR> function)
            => task => task.Select(function);

        public static Result<T, TE> Flattern<T, TE>(this Result<Result<T, TE>, TE> resultResult) 
            => resultResult.SelectMany(Identity);
        #endregion

        #region Exception Wrapping
        public static Result<T, Exception> Try<T>(Func<T> task)
        {
            try
            {
                return Result<T, Exception>.Create(task());
            }
            catch (Exception e)
            {
                return Result<T, Exception>.Raise(e);
            }
        }
        #endregion
    }
}
