using Xunit;

namespace Woz.Functional.Tests
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
