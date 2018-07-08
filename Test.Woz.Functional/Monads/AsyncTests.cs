using System;
using Woz.Functional.Monads;
using System.Threading.Tasks;
using Xunit;

namespace Test.Woz.Functional.Monads
{
    public class AsyncTests
    {
        [Fact]
        public void SelectManySimple()
        {
            var task = MakeTask(5).SelectMany(value => MakeTask(value + 3));

            task.Wait();
            Assert.Equal(8, task.Result);
        }

        [Fact]
        public void SelectManyFull()
        {
            var task =
                from a in MakeTask(5)
                from b in MakeTask(3)
                select a + b;

            task.Wait();
            Assert.Equal(8, task.Result);
        }

        [Fact]
        public void Select()
        {
            var task = MakeTask(5).Select(x => x + 3);

            task.Wait();
            Assert.Equal(8, task.Result);
        }

        private Task<T> MakeTask<T>(T value)
        {
            Task.Delay(20);
            return value.ToTask();
        }

        [Fact]
        public void Flattern()
        {
            var task = MakeTask(MakeTask(5)).Flattern();

            task.Wait();
            Assert.Equal(5, task.Result);
        }
    }
}
