using System;
using Woz.Functional.Monads;
using Xunit;

namespace Test.Woz.Functional.Monads
{
    public class ResultTests
    {
        private const string ErrorMessage = "Bang";
        private readonly Result<int, string> ErrorResult = Result<int, string>.Raise(ErrorMessage);

        [Fact]
        public void ValueState()
        {
            var result = Result<int, string>.Create(5);
            Assert.Equal(5, result.Value);
            Assert.True(result.HasValue);
            Assert.Throws<InvalidOperationException>(() => result.Error);
        }

        [Fact]
        public void ErrorState()
        {
            Assert.Equal("Bang", ErrorResult.Error);
            Assert.False(ErrorResult.HasValue);
            Assert.Throws<InvalidOperationException>(() => ErrorResult.Value);
        }

        [Fact]
        public void SelectManySimple()
        {
            var result = Result<int, string>.Create(5).SelectMany(value => Result<int, string>.Create(value + 3));

            Assert.Equal(8, result.Value);
        }

        [Fact]
        public void SelectManyBothValue()
        {
            var result =
                from a in Result<int, string>.Create(5)
                from b in Result<int, string>.Create(3)
                select a + b;

            Assert.Equal(8, result.Value);
        }

        [Fact]
        public void SelectManyFirstError()
        {
            var result =
                from a in ErrorResult
                from b in Result<int, string>.Create(3)
                select a + b;

            Assert.False(result.HasValue);
        }

        [Fact]
        public void SelectManySecondError()
        {
            var result =
                from a in Result<int, string>.Create(5)
                from b in ErrorResult
                select a + b;

            Assert.False(result.HasValue);
        }

        [Fact]
        public void Select()
        {
            Assert.Equal(6, Result<int, string>.Create(5).Select(value => value + 1).Value);
            Assert.False(ErrorResult.Select(x => x + 1).HasValue);
        }

        [Fact]
        public void Try()
        {
            var exception = new Exception("Bang");

            Assert.Equal(5, Result.Try(() => 5).Value);
            Assert.Same(exception, Result.Try<int>(() => throw exception).Error);
        }
    }
}
