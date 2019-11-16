using System.Linq;
using Woz.Functional.Monads;
using Xunit;

namespace Woz.Functional.Tests.Monads
{
    public class MaybeEnumerableTests
    {
        private static readonly Maybe<int> Some5 = 5.ToSome();

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
    }
}
