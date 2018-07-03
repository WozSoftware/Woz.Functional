using System;
using System.Collections.Generic;
using System.Text;
using Woz.Functional.Monads;
using Xunit;

namespace Test.Woz.Functional.Monads
{
    public class StateTests
    {
        [Fact]
        public void Construction()
        {
            const string stateInstance = "SomeState";
            const int value = 5;

            var state = value.ToState<string, int>();
            var result = state(stateInstance);

            Assert.Equal(stateInstance, result.Item1);
            Assert.Equal(value, result.Item2);
        }

        [Fact]
        public void SelectManySimple()
        {
            const string stateInstance = "SomeState";
            const string stateUpdate = "Updated";
            const int value = 5;

            var state = value.ToState<string, int>().SelectMany<string, int, int>(v => s => (s + stateUpdate, v + 1));

            Assert.Equal(stateInstance + stateUpdate, state(stateInstance).Item1);
            Assert.Equal(value + 1, state(stateInstance).Item2);
        }

        [Fact]
        public void SelectManyFull()
        {
            const string stateInstance = "SomeState";

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
            const int value = 5;

            var state = value.ToState<string, int>().Select(v => v + 1);

            Assert.Equal(stateInstance, state(stateInstance).Item1);
            Assert.Equal(value + 1, state(stateInstance).Item2);
        }
    }
}
