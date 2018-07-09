﻿using System;
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
        public void Flattern()
        {
            var task = MakeTask(MakeTask(5)).Flattern();

            task.Wait();
            Assert.Equal(5, task.Result);
        }
    }
}
