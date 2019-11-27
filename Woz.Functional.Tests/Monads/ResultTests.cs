using System;
using Woz.Functional.Monads;
using Xunit;

namespace Woz.Functional.Tests.Monads
{
    public class ResultTests
    {
        private const string ErrorMessage = "Bang";
        private static readonly Result<int, string> ErrorResult = Result<int, string>.Raise(ErrorMessage);

        private static readonly Func<int, Result<decimal, string>> Function1 =
            value => Result<decimal, string>.Create(value);

        private static readonly Func<decimal, Result<int, string>> Function2 =
            value => Result<int, string>.Create((int)value + 1);


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
            Assert.Equal(ErrorMessage, ErrorResult.Error);
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
        public void KleisliInto() => Assert.Equal(6, Function1.Into(Function2)(5).Value);

        [Fact]
        public void KleisliIntoProjected() => Assert.Equal(11, Function1.Into(Function2, (a, b) => a + b)(5).Value);

        [Fact]
        public void Lift()
        {
            static string func(int value) => value.ToString();
            var liftedFunc = Result.Lift<int, string, string>(func);

            Assert.Equal("5", liftedFunc(Result<int, string>.Create(5)).Value);
            Assert.False(liftedFunc(ErrorResult).HasValue);
        }

        [Fact]
        public void Apply()
        {
            static string func(int value) => value.ToString();

            Assert.Equal(
                "5",
                Result<int, string>.Create(5).Apply(Result<Func<int, string>, string>.Create(func)).Value);
        }

        [Fact]
        public void Flattern()
        {
            // generic vomit :)
            var resultResult = Result<Result<int, string>, string>.Create(Result<int, string>.Create(5));
            var ErrorResult = Result<Result<int, string>, string>.Raise("Bang");

            Assert.Equal(5, resultResult.Flattern().Value);
            Assert.False(ErrorResult.Flattern().HasValue);

        }

        [Fact]
        public void Try()
        {
            var exception = new Exception("Bang");

            Assert.Equal(5, Result.Try(() => 5).Value);
            Assert.Same(exception, Result.Try<int>(() => throw exception).Error);
        }

        [Fact]
        public void MatchMatchStyle()
        {
            static string someFunc(int x) => x.ToString();
            static string noneFunc(string e) => e;

            Assert.Equal("5", Result<int, string>.Create(5).Match(someFunc, noneFunc));
            Assert.Equal(ErrorMessage, ErrorResult.Match(someFunc, noneFunc));
        }

        [Fact]
        public void MatchBindStyle()
        {
            static Result<string, string> someFunc(int x) => Result<string, string>.Create(x.ToString());
            static Result<string, string> noneFunc(string e) => Result<string, string>.Raise(e);

            Assert.Equal("5", Result<int, string>.Create(5).Match(someFunc, noneFunc).Value);
            Assert.False(ErrorResult.Match(someFunc, noneFunc).HasValue);
        }

        [Fact]
        public void Tee()
        {
            var value = 0;
            Assert.Equal(5, Result<int, string>.Create(5).Tee(x => value = x).Value);
            Assert.Equal(5, value);

            Assert.False(ErrorResult.Tee(x => { throw new Exception(); }).HasValue);
        }

    }
}
