using System.Collections.Generic;
using Woz.Functional.Monads;
using Xunit;

namespace Test.Woz.Functional.Monads
{
    public class MaybeDictionaryTests
    {
        [Fact]
        public void MaybeFind()
        {
            var dict = new Dictionary<int, string> { [1] = "A", [2] = "B" };

            Assert.Equal("A", dict.MaybeFind(1).Value);
            Assert.False(dict.MaybeFind(3).HasValue);
        }
    }
}
