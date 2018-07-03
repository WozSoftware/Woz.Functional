using System;

namespace Woz.Functional.Monads
{
    public delegate (TS, TV) State<TS, TV>(TS state);

    public static class StateExtensions
    {
        #region Construction
        public static State<TS, TV> ToState<TS, TV>(this TV value) => state => (state, value);
        #endregion

        #region LINQ=Map/Flatmap
        public static State<TS, TR> SelectMany<TS, T, TR>(
            this State<TS, T> source, Func<T, State<TS, TR>> selector) 
            => state =>
            {
                var sourceResult = source(state);
                return selector(sourceResult.Item2)(sourceResult.Item1);
            };

        public static State<TS, TR> SelectMany<TS, T1, T2, TR>(
            this State<TS, T1> source, Func<T1, State<TS, T2>> selector, Func<T1, T2, TR> projection)
            => source.SelectMany(
                value1 => selector(value1).SelectMany<TS, T2, TR>(
                    value2 => state => (state, projection(value1, value2))));

        public static State<TS, TR> Select<TS, T, TR>(
            this State<TS, T> source, Func<T, TR> selector)
            => source.SelectMany<TS, T, TR>(value => state => (state, selector(value)));
        #endregion
    }
}
