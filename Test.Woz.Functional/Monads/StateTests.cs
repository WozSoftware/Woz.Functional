using System;
using Woz.Functional.Monads;
using static Woz.Functional.Monads.State;
using Xunit;

namespace Test.Woz.Functional.Monads
{
    public class StateTests
    {
        private const string stateInstance = "SomeState";
        private const string stateUpdate = "Updated";
        private const int value = 5;

        [Fact]
        public void Construction()
        {
            var state = value.ToState<string, int>();
            var result = state(stateInstance);

            Assert.Equal(stateInstance, result.Item1);
            Assert.Equal(value, result.Item2);
        }

        [Fact]
        public void SelectManySimple()
        {
            var state = value.ToState<string, int>().SelectMany<string, int, int>(v => s => (s + stateUpdate, v + 1));

            Assert.Equal(stateInstance + stateUpdate, state(stateInstance).Item1);
            Assert.Equal(value + 1, state(stateInstance).Item2);
        }

        [Fact]
        public void SelectManyFull()
        {
            var state =
                from a in 5.ToState<string, int>()
                from b in 3.ToState<string, int>()
                select a + b;

            Assert.Equal(stateInstance, state(stateInstance).Item1);
            Assert.Equal(8, state(stateInstance).Item2);
        }

        [Fact]
        public void Select()
        {
            const string stateInstance = "SomeState";

            var state = value.ToState<string, int>().Select(v => v + 1);

            Assert.Equal(stateInstance, state(stateInstance).Item1);
            Assert.Equal(value + 1, state(stateInstance).Item2);
        }

        [Fact]
        public void KleisliIntoSimple()
        {
            var composed = Function1.Into(Function2);
            var result = composed(value)(stateInstance);

            Assert.Equal(stateInstance, result.Item1);
            Assert.Equal(value + 1, result.Item2);
        }

        [Fact]
        public void KleisliSelectManyFull()
        {
            var composed = Function1.Into(Function2, (a, b) => a + b);

            var result = composed(value)(stateInstance);

            Assert.Equal(stateInstance, result.Item1);
            Assert.Equal(value + (value + 1), result.Item2);
        }

        [Fact]
        public void Lift()
        {
            Func<int, string> func = value => value.ToString();
            var result = State.Lift<string, int, string>(func)(5.ToState<string, int>())(stateInstance);

            Assert.Equal(stateInstance, result.Item1);
            Assert.Equal("5", result.Item2);
        }

        [Fact]
        public void Flattern()
        {
            bool evaluated = false;

            var result = new Lazy<Lazy<int>>(
                () => new Lazy<int>(() => { evaluated = true; return 5; }))
                .Flattern();

            Assert.False(evaluated);
            Assert.Equal(5, result.Value);
            Assert.True(evaluated);
        }

        [Fact]
        public void GetState() 
            => Assert.Equal(stateInstance, Function1(5).SelectMany(_ => GetState<string>())(stateInstance).Item2);

        [Fact]
        public void SetState() 
            => Assert.Equal("New", Function1(5).SelectMany(_ => SetState<string>("New"))(stateInstance).Item1);

        private static readonly Func<int, State<string, int>> Function1 = 
            value => value.ToState<string, int>();

        private static readonly Func<int, State<string, int>> Function2 = 
            value => value.ToState<string, int>().Select(v => v + 1);
    }
}
