using Woz.Functional;
using Xunit;

namespace Test.Woz.Functional
{
    public class UnitTests
    {
        [Fact]
        public void AllThatIsRequired()
        {
            Assert.Equal(Unit.Value, Unit.Value);
        }
    }
}
