﻿using System;
using Woz.Functional.Monads;
using Xunit;

namespace Woz.Functional.Tests.Monads
{
    public class LazyTests
    {
        private static readonly Func<int, Lazy<decimal>> Function1 = value => ((decimal)value).ToLazy();
        private static readonly Func<decimal, Lazy<int>> Function2 = value => ((int)value + 1).ToLazy();

        [Fact]
        public void SelectManySimple()
            => TestLazy<int>(
                makeLazy => makeLazy(5).SelectMany(value => makeLazy(value + 3)),
                lazy => Assert.Equal(8, lazy.Value));

        [Fact]
        public void SelectManyFull()
            => TestLazy<int>(
                makeLazy =>
                    from a in makeLazy(5)
                    from b in makeLazy(3)
                    select a + b,
                lazy => Assert.Equal(8, lazy.Value));

        [Fact]
        public void Select()
            => TestLazy<int>(
                makeLazy => makeLazy(5).Select(x => x + 3),
                lazy => Assert.Equal(8, lazy.Value));

        private static void TestLazy<T>(
            Func<Func<T, Lazy<T>>, Lazy<T>> factory, Action<Lazy<T>> tester)
        {
            bool evaluated = false;

            Lazy<T> makeLazy(T value) => new Lazy<T>(() => { evaluated = true; return value; });

            var result = factory(makeLazy);

            Assert.False(evaluated);
            tester(result);
            Assert.True(evaluated);
        }

        [Fact]
        public void KleisliInto() => Assert.Equal(6, Function1.Into(Function2)(5).Value);

        [Fact]
        public void KleisliSelectManyFull() => Assert.Equal(11, Function1.Into(Function2, (a, b) => a + b)(5).Value);

        [Fact]
        public void Lift()
        {
            Func<int, string> func = value => value.ToString();
            Assert.Equal("5", Lazy.Lift(func)(5.ToLazy()).Value);
        }

        [Fact]
        public void Apply()
        {
            Func<int, string> func = value => value.ToString();
            Assert.Equal("5", 5.ToLazy().Apply(func.ToLazy()).Value);
        }

        [Fact]
        public void Flattern()
        {
            bool evaluated = false;

            var result = new Lazy<Lazy<int>>(
                () => new Lazy<int>(() => { evaluated = true; return 5; }))
                .Flattern();

            Assert.False(evaluated);
            Assert.Equal(5, result.Value);
            Assert.True(evaluated);
        }
    }
}
