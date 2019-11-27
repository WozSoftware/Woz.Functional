using System;
using Woz.Functional.Monads;
using Xunit;

namespace Woz.Functional.Tests.Monads
{
    public class ReaderTests
    {
        private class Env { public int A; }

        private static readonly Func<int, Reader<Env, int>> Increment = value => env => value + 1;

        private static readonly Env _env = new Env { A = 5 };
        private static readonly Func<Env, int> _getA = env => env.A;
        private static readonly Reader<Env, int> _getAReader = _getA.ToReader();

        [Fact]
        public void Construction()
        {
            Assert.Equal(5, 5.ToReader<Env, int>()(_env));
            Assert.Equal(5, _getAReader(_env));
        }

        [Fact]
        public void SelectMany() => Assert.Equal(6, _getAReader.SelectMany(Increment)(_env));

        [Fact]
        public void SelectManyFull()
        {
            var reader =
                from a in _getAReader
                from b in Increment(a)
                select b;

            Assert.Equal(6, reader(_env));
        }

        [Fact]
        public void Select() => Assert.Equal(6, _getAReader.Select(x => x + 1)(_env));

        [Fact]
        public void KleisliInto() => Assert.Equal(7, Increment.Into(Increment)(5)(_env));

        [Fact]
        public void KleisliSelectManyFull()
            => Assert.Equal(13, Increment.Into(Increment, (a, b) => a + b)(5)(_env));

        [Fact]
        public void Lift()
        {
            static string func(int val) => val.ToString();
            Assert.Equal("5", Reader.Lift<Env, int, string>(func)(_getAReader)(_env));
        }

        [Fact]
        public void Flattern()
            => Assert.Equal(5, _getAReader.ToReader<Env, Reader<Env, int>>().Flattern()(_env));
    }
}
