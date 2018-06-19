using System;
using Woz.Functional.Monads;
using Xunit;

namespace Test.Woz.Functional.Monads
{
    public class MaybeTests
    {
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
            var maybe = 5.ToSome();
            Assert.Equal(5, maybe.Value);
            Assert.True(maybe.HasValue);
        }

        [Fact]
        public void NoneState()
        {
            Assert.False(Maybe<int>.None.HasValue);
            Assert.Throws<InvalidOperationException>(() => Maybe<int>.None.Value);
        }

        [Fact]
        public void SelectManyBothSome()
        {
            var result =
                from a in 5.ToSome()
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
            Assert.Equal(6, 5.ToSome().Select(x => x + 1).Value);
            Assert.False(Maybe<int>.None.Select(x => x + 1).HasValue);
        }

        [Fact]
        public void Where()
        {
            Assert.Equal(5, 5.ToSome().Where(_ => true).Value);
            Assert.False(5.ToSome().Where(_ => false).HasValue);
            Assert.False(Maybe<int>.None.Where(_ => true).HasValue);
            Assert.False(Maybe<int>.None.Where(_ => false).HasValue);
        }

        [Fact]
        public void RecoverValue()
        {
            Assert.Equal(5, 5.ToSome().Recover(7).Value);
            Assert.Equal(7, Maybe<int>.None.Recover(7).Value);
        }
    }
}
