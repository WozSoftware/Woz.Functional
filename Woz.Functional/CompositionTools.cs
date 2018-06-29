using System;

namespace Woz.Functional
{
    public static class CompositionTools
    {
        #region Compose Into Func
        public static Func<T1, TGR> 
            Into<T1, TFR, TGR>(this Func<T1, TFR> f, Func<TFR, TGR> g)
            => v1 => g(f(v1));

        public static Func<T1, T2, TGR> 
            Into<T1, T2, TFR, TGR>(this Func<T1, T2, TFR> f, Func<TFR, TGR> g)
            => (v1, v2) => g(f(v1, v2));

        public static Func<T1, T2, T3, TGR>
            Into<T1, T2, T3, TFR, TGR>(this Func<T1, T2, T3, TFR> f, Func<TFR, TGR> g)
            => (v1, v2, v3) => g(f(v1, v2, v3));

        public static Func<T1, T2, T3, T4, TGR>
            Into<T1, T2, T3, T4, TFR, TGR>(this Func<T1, T2, T3, T4, TFR> f, Func<TFR, TGR> g)
            => (v1, v2, v3, v4) => g(f(v1, v2, v3, v4));

        public static Func<T1, T2, T3, T4, T5, TGR>
            Into<T1, T2, T3, T4, T5, TFR, TGR>(this Func<T1, T2, T3, T4, T5, TFR> f, Func<TFR, TGR> g)
            => (v1, v2, v3, v4, v5) => g(f(v1, v2, v3, v4, v5));
        #endregion 

        #region Compose Into Action
        public static Action<T1>
            Into<T1, TFR>(this Func<T1, TFR> f, Action<TFR> g)
            => v1 => g(f(v1));

        public static Action<T1, T2>
            Into<T1, T2, TFR>(this Func<T1, T2, TFR> f, Action<TFR> g)
            => (v1, v2) => g(f(v1, v2));

        public static Action<T1, T2, T3>
            Into<T1, T2, T3, TFR>(this Func<T1, T2, T3, TFR> f, Action<TFR> g)
            => (v1, v2, v3) => g(f(v1, v2, v3));

        public static Action<T1, T2, T3, T4>
            Into<T1, T2, T3, T4, TFR>(this Func<T1, T2, T3, T4, TFR> f, Action<TFR> g)
            => (v1, v2, v3, v4) => g(f(v1, v2, v3, v4));

        public static Action<T1, T2, T3, T4, T5>
            Into<T1, T2, T3, T4, T5, TFR>(this Func<T1, T2, T3, T4, T5, TFR> f, Action<TFR> g)
            => (v1, v2, v3, v4, v5) => g(f(v1, v2, v3, v4, v5));
        #endregion 

        #region Reverse Args
        public static Func<T2, T1, TR> 
            ReverseArgs<T1, T2, TR>(this Func<T1, T2, TR> func)
            => (v2, v1) => func(v1, v2);

        public static Func<T3, T2, T1, TR> 
            ReverseArgs<T1, T2, T3, TR>(this Func<T1, T2, T3, TR> func)
            => (v3, v2, v1) => func(v1, v2, v3);

        public static Func<T4, T3, T2, T1, TR> 
            ReverseArgs<T1, T2, T3, T4, TR>(this Func<T1, T2, T3, T4, TR> func)
            => (v4, v3, v2, v1) => func(v1, v2, v3, v4);

        public static Func<T5, T4, T3, T2, T1, TR>
            ReverseArgs<T1, T2, T3, T4, T5, TR>(this Func<T1, T2, T3, T4, T5, TR> func)
            => (v5, v4, v3, v2, v1) => func(v1, v2, v3, v4, v5);
        #endregion

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
