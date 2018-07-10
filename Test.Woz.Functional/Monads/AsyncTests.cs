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
            => TestTask(
                MakeTask(5).SelectMany(value => MakeTask(value + 3)),
                task => Assert.Equal(8, task.Result));

        [Fact]
        public void SelectManyFull() 
            => TestTask(
                from a in MakeTask(5)
                from b in MakeTask(3)
                select a + b,
                task => Assert.Equal(8, task.Result));

        [Fact]
        public void Select() 
            => TestTask(
                MakeTask(5).Select(x => x + 3),
                task => Assert.Equal(8, task.Result));

        private void TestTask<T>(Task<T> task, Action<Task<T>> tester)
        {
            task.Wait(100);
            Assert.True(task.IsCompletedSuccessfully);
            tester(task);
        }

        private Task<T> MakeTask<T>(T value) 
            => Task.Run(
                () =>
                {
                    Task.Delay(20);
                    return value;
                });

        [Fact]
        public void KleisliInto()
        {
            var composed = Function1.Into(Function2);

            Assert.Equal(6, composed(5).Result);
        }

        [Fact]
        public void KleisliIntoProjected()
        {
            var composed = Function1.Into(Function2, (a, b) => a + b);

            Assert.Equal(11, composed(5).Result);
        }

        public static readonly Func<int, Task<decimal>> Function1 = value => ((decimal)value).ToTask();
        public static readonly Func<decimal, Task<int>> Function2 = value => (((int)value) + 1).ToTask();

        [Fact]
        public void Flattern()
        {
            var task = MakeTask(MakeTask(5)).Flattern();

            task.Wait();
            Assert.Equal(5, task.Result);
        }
    }
}
