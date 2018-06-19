using Woz.Functional;
using Xunit;

namespace Test.Woz.Functional
{
    public class UnitTests
    {
        [Fact]
        public void AllThatItRequired()
        {
            Assert.Equal(Unit.Value, Unit.Value);
        }
    }
}
