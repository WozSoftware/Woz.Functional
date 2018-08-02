using System;
using System.Collections.Generic;
using System.Linq;
using Woz.Functional.Monads;
using Xunit;

namespace Test.Woz.Functional.Monads
{
    public class MaybeTests
    {
        private static readonly Maybe<int> Some5 = 5.ToSome();

        [Fact]
        public void Construction()
        {
            Assert.Equal(5, 5.ToSome().Value);
            Assert.Equal(5, 5.ToMaybe().Value);
            Assert.Equal(Maybe<int>.None.HasValue, ((int?)null).ToMaybe().HasValue);
        }

        [Fact]
        public void SomeState()
        {
            Assert.Equal(5, Some5.Value);
            Assert.True(Some5.HasValue);
        }

        [Fact]
        public void NoneState()
        {
            Assert.False(Maybe<int>.None.HasValue);
            Assert.Throws<InvalidOperationException>(() => Maybe<int>.None.Value);
        }

        [Fact]
        public void SelectManySimple()
        {
            var result = Some5.SelectMany(value => (value + 3).ToSome());

            Assert.Equal(8, result.Value);
        }

        [Fact]
        public void SelectManyBothSome()
        {
            var result =
                from a in Some5
                from b in 3.ToSome()
                select a + b;

            Assert.Equal(8, result.Value);
        }

        [Fact]
        void SelectManyFirstNone()
        {
            var result =
                from a in Maybe<int>.None
                from b in 3.ToSome()
                select a + b;

            Assert.False(result.HasValue);
        }

        [Fact]
        void SelectManySecondNone()
        {
            var result =
                from a in 3.ToSome()
                from b in Maybe<int>.None
                select a + b;

            Assert.False(result.HasValue);
        }

        [Fact]
        public void Select()
        {
            Assert.Equal(6, Some5.Select(x => x + 1).Value);
            Assert.False(Maybe<int>.None.Select(x => x + 1).HasValue);
        }

        [Fact]
        public void KleisliInto() => Assert.Equal(6, Function1.Into(Function2)(5).Value);

        [Fact]
        public void KleisliSelectManyFull() => Assert.Equal(11, Function1.Into(Function2, (a, b) => a + b)(5).Value);

        private static readonly Func<int, Maybe<decimal>> Function1 = value => ((decimal)value).ToSome();
        private static readonly Func<decimal, Maybe<int>> Function2 = value => (((int)value) + 1).ToSome();

        [Fact]
        public void Where()
        {
            Assert.Equal(5, Some5.Where(_ => true).Value);
            Assert.False(Some5.Where(_ => false).HasValue);
            Assert.False(Maybe<int>.None.Where(_ => true).HasValue);
            Assert.False(Maybe<int>.None.Where(_ => false).HasValue);
        }

        [Fact]
        public void Lift()
        {
            Func<int, string> func = value => value.ToString();
            var liftedFunc = Maybe.Lift(func);
            Assert.Equal("5", liftedFunc(Some5).Value);
            Assert.False(liftedFunc(Maybe<int>.None).HasValue);
        }

        [Fact]
        public void Apply()
        {
            Func<int, string> func = value => value.ToString();
            Assert.Equal("5", Some5.Apply(func.ToSome()).Value);
        }

        [Fact]
        public void Flattern()
        {
            Assert.Equal(5, Some5.ToSome().Flattern().Value);
            Assert.False(Some5.Where(_ => false).HasValue);
            Assert.False(Maybe<int>.None.Where(_ => true).HasValue);
            Assert.False(Maybe<int>.None.Where(_ => false).HasValue);
        }

        [Fact]
        public void MatchMatchStyle()
        {
            const string failed = "failed";
            string someFunc(int x) => x.ToString();
            string noneFunc() => failed;

            Assert.Equal("5", Some5.Match(someFunc, noneFunc));
            Assert.Equal(failed, Maybe<int>.None.Match(someFunc, noneFunc));
        }

        [Fact]
        public void MatchBindStyle()
        {
            Maybe<string> someFunc(int x) => x.ToString().ToSome();
            Maybe<string> noneFunc() => Maybe<string>.None;

            Assert.Equal("5", Some5.Match(someFunc, noneFunc).Value);
            Assert.False(Maybe<int>.None.Match(someFunc, noneFunc).HasValue);
        }

        [Fact]
        public void Tee()
        {
            var value = 0;
            Assert.Equal(5, Some5.Tee(x => value = x).Value);
            Assert.Equal(5, value);

            Assert.False(Maybe<int>.None.Tee(x => { throw new Exception(); }).HasValue);
        }

        [Fact]
        public void RecoverValue()
        {
            Assert.Equal(5, Some5.Recover(7).Value);
            Assert.Equal(7, Maybe<int>.None.Recover(7).Value);
        }

        [Fact]
        public void RecoverValueFactory()
        {
            Assert.Equal(5, Some5.Recover(() => 7).Value);
            Assert.Equal(7, Maybe<int>.None.Recover(() => 7).Value);
        }

        [Fact]
        public void ToEnumerable()
        {
            var some = Some5.ToEnumerable();
            Assert.Single(some);
            Assert.Equal(5, some.First());

            Assert.False(Maybe<int>.None.ToEnumerable().Any());
        }

        [Fact]
        public void MaybeMin()
        {
            Assert.Equal(1, new[] { 3, 4, 1, 6, 2 }.MaybeMin().Value);
            Assert.Equal(1, new[] { new { Id = 2 }, new { Id = 1 } }.MaybeMin(x => x.Id).Value);
            Assert.False(new int[0].MaybeMin().HasValue);
        }

        [Fact]
        public void MaybeMax()
        {
            Assert.Equal(6, new[] { 3, 4, 1, 6, 2 }.MaybeMax().Value);
            Assert.Equal(2, new[] { new { Id = 2 }, new { Id = 1 } }.MaybeMax(x => x.Id).Value);
            Assert.False(new int[0].MaybeMax().HasValue);
        }

        [Fact]
        public void MaybeFind()
        {
            var dict = new Dictionary<int, string> { [1] = "A", [2] = "B" };

            Assert.Equal("A", dict.MaybeFind(1).Value);
            Assert.False(dict.MaybeFind(3).HasValue);
        }
    }
}
