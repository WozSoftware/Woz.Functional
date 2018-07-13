using System;
using static Woz.Functional.FreeFunctions;

namespace Woz.Functional.Monads
{
    public delegate (TS, TV) State<TS, TV>(TS state);

    public static class State
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
            this State<TS, T1> source, 
            Func<T1, State<TS, T2>> selector, 
            Func<T1, T2, TR> projection)
            => source.SelectMany(
                value1 => selector(value1).SelectMany<TS, T2, TR>(
                    value2 => state => (state, projection(value1, value2))));

        public static State<TS, TR> Select<TS, T, TR>(
            this State<TS, T> source, Func<T, TR> selector)
            => source.SelectMany<TS, T, TR>(value => state => (state, selector(value)));
        #endregion

        #region Kleisli
        public static Func<T1, State<TS, TR>> Into<TS, T1, T2, TR>(
            this Func<T1, State<TS, T2>> f, Func<T2, State<TS, TR>> g)
            => value => f(value).SelectMany(g);

        public static Func<T1, State<TS, TR>> Into<TS, T1, T2, T3, TR>(
            this Func<T1, State<TS, T2>> f, Func<T2, State<TS, T3>> g, Func<T2, T3, TR> projection)
            => value => f(value).SelectMany(g, projection);
        #endregion

        #region Utility
        public static Func<State<TS, T>, State<TS, TR>> Lift<TS, T, TR>(this Func<T, TR> function)
            => task => task.Select(function);

        public static State<TS, T> Flattern<TS, T>(this State<TS, State<TS, T>> taskTask) 
            => taskTask.SelectMany(Identity);
        #endregion

        #region State Manipulation
        public static State<TS, TS> GetState<TS>() => state => (state, state);

        public static State<TS, Unit> SetState<TS>(TS newState) => state => (newState, Unit.Value);
        #endregion
    }
}
