using System;
using Woz.Functional.Monads;
using Xunit;

namespace Test.Woz.Functional.Monads
{
    public class IOTests
    {
        [Fact]
        public void Construction() => Assert.Equal(5, Get5IO());

        [Fact]
        public void SelectMany() => Assert.Equal(6, Get5IO.SelectMany(Increment)());

        [Fact]
        public void SelectManyFull()
        {
            var io =
                from a in Get5IO
                from b in Increment(a)
                select b;

            Assert.Equal(6, io());
        }

        [Fact]
        public void Select() => Assert.Equal(6, Get5IO.Select(x => x + 1)());

        [Fact]
        public void KleisliInto() => Assert.Equal(7, Increment.Into(Increment)(5)());

        [Fact]
        public void KleisliSelectManyFull() => Assert.Equal(13, Increment.Into(Increment, (a, b) => a + b)(5)());

        [Fact]
        public void Lift()
        {
            Func<int, string> func = value => value.ToString();
            Assert.Equal("5", IO.Lift(func)(Get5IO).Run().Value);
        }

        [Fact]
        public void Flattern() => Assert.Equal(5, new IO<IO<int>>(() => Get5IO).Flattern().Run().Value);

        [Fact]
        public void Run()
        {
            Assert.Equal(5, Get5IO.Run().Value);
            Assert.Equal("Bang", BangIO.Run().Error.Message);
        }

        private static Func<int, IO<int>> Increment = value => () => value + 1;

        private static readonly Func<int> Get5 = () => 5;
        private static readonly IO<int> Get5IO = Get5.ToIO();

        private static readonly Func<int> Bang = () => { throw new Exception("Bang"); };
        private static readonly IO<int> BangIO = Bang.ToIO();
    }
}
