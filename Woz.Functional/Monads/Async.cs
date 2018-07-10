﻿using System;
using System.Threading.Tasks;
using static Woz.Functional.FreeFunctions;

namespace Woz.Functional.Monads
{
    public static class Async
    {
        #region Construction
        public static Task<T> ToTask<T>(this T value) => Task.FromResult(value);
        public static async Task<T> ToTask<T>(this Func<T> task) => await Task.Run(task);
        #endregion

        #region LINQ=Map/Flatmap
        public static async Task<TR> SelectMany<T, TR>(this Task<T> task, Func<T, Task<TR>> selector) 
            => await selector(await task);

        public static Task<TR> SelectMany<T1, T2, TR>(
            this Task<T1> task, Func<T1, Task<T2>> selector, Func<T1, T2, TR> projection)
            => task.SelectMany(
                value1 => selector(value1).SelectMany(value2 => projection(value1, value2).ToTask()));

        public static Task<TR> Select<T, TR>(this Task<T> task, Func<T, TR> selector)
            => task.SelectMany(value => selector(value).ToTask());
        #endregion

        #region Kleisli
        public static Func<T1, Task<TR>> Into<T1, T2, TR>(
            this Func<T1, Task<T2>> f, Func<T2, Task<TR>> g)
            => value => f(value).SelectMany(g);

        public static Func<T1, Task<TR>> Into<T1, T2, T3, TR>(
            this Func<T1, Task<T2>> f, Func<T2, Task<T3>> g, Func<T2, T3, TR> projection)
            => value => f(value).SelectMany(g, projection);
        #endregion

        #region Utility
        public static Task<T> Flattern<T>(this Task<Task<T>> taskTask) => taskTask.SelectMany(Identity);
        #endregion
    }
}
