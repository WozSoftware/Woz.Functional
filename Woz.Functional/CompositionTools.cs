using System;

namespace Woz.Functional
{
    public static class CompositionTools
    {
        #region Curry Func
        public static Func<T1, Func<T2, TR>> 
            Curry<T1, T2, TR>(this Func<T1, T2, TR> function)
            => v1 => v2 => function(v1, v2);

        public static Func<T1, Func<T2, Func<T3, TR>>> 
            Curry<T1, T2, T3, TR>(this Func<T1, T2, T3, TR> function)
            => v1 => v2 => v3 => function(v1, v2, v3);

        public static Func<T1, Func<T2, Func<T3, Func<T4, TR>>>>
            Curry<T1, T2, T3, T4, TR>(this Func<T1, T2, T3, T4, TR> function)
            => v1 => v2 => v3 => v4 => function(v1, v2, v3, v4);

        public static Func<T1, Func<T2, Func<T3, Func<T4, Func<T5, TR>>>>>
            Curry<T1, T2, T3, T4, T5, TR>(this Func<T1, T2, T3, T4, T5, TR> function)
            => v1 => v2 => v3 => v4 => v5 => function(v1, v2, v3, v4, v5);
        #endregion

        #region Curry Action
        public static Func<T1, Action<T2>>
            Curry<T1, T2>(this Action<T1, T2> action)
            => v1 => v2 => action(v1, v2);

        public static Func<T1, Func<T2, Action<T3>>>
            Curry<T1, T2, T3>(this Action<T1, T2, T3> action)
            => v1 => v2 => v3 => action(v1, v2, v3);

        public static Func<T1, Func<T2, Func<T3, Action<T4>>>>
            Curry<T1, T2, T3, T4>(this Action<T1, T2, T3, T4> action)
            => v1 => v2 => v3 => v4 => action(v1, v2, v3, v4);

        public static Func<T1, Func<T2, Func<T3, Func<T4, Action<T5>>>>>
            Curry<T1, T2, T3, T4, T5>(this Action<T1, T2, T3, T4, T5> action)
            => v1 => v2 => v3 => v4 => v5 => action(v1, v2, v3, v4, v5);
        #endregion

        #region DeCurry Func
        public static Func<T1, T2, TR>
            DeCurry<T1, T2, TR>(this Func<T1, Func<T2, TR>> function)
            => (v1, v2) => function(v1)(v2);

        public static Func<T1, T2, T3, TR>
            DeCurry<T1, T2, T3, TR>(this Func<T1, Func<T2, Func<T3, TR>>> function)
            => (v1, v2, v3) => function(v1)(v2)(v3);

        public static Func<T1, T2, T3, T4, TR>
            DeCurry<T1, T2, T3, T4, TR>(this Func<T1, Func<T2, Func<T3, Func<T4, TR>>>> function)
            => (v1, v2, v3, v4) => function(v1)(v2)(v3)(v4);

        public static Func<T1, T2, T3, T4, T5, TR>
            DeCurry<T1, T2, T3, T4, T5, TR>(this Func<T1, Func<T2, Func<T3, Func<T4, Func<T5, TR>>>>> function)
            => (v1, v2, v3, v4, v5) => function(v1)(v2)(v3)(v4)(v5);
        #endregion

        #region DeCurry Action
        public static Action<T1, T2>
            DeCurry<T1, T2>(this Func<T1, Action<T2>> action)
            => (v1, v2) => action(v1)(v2);

        public static Action<T1, T2, T3>
            DeCurry<T1, T2, T3>(this Func<T1, Func<T2, Action<T3>>> action)
            => (v1, v2, v3) => action(v1)(v2)(v3);

        public static Action<T1, T2, T3, T4>
            DeCurry<T1, T2, T3, T4>(this Func<T1, Func<T2, Func<T3, Action<T4>>>> action)
            => (v1, v2, v3, v4) => action(v1)(v2)(v3)(v4);

        public static Action<T1, T2, T3, T4, T5>
            DeCurry<T1, T2, T3, T4, T5>(this Func<T1, Func<T2, Func<T3, Func<T4, Action<T5>>>>> action)
            => (v1, v2, v3, v4, v5) => action(v1)(v2)(v3)(v4)(v5);
        #endregion
    }
}
