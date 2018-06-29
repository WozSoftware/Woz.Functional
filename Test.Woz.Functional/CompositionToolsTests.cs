using System;
using Woz.Functional;
using Xunit;

namespace Test.Woz.Functional
{
    public class CompositionToolsTests
    {
        // NOTE: These feel long winded but they ensure the functions are pulled apart
        // and put together correctly

        private readonly Func<int, int, bool> _func2Args =
            (a, b) => a == 1 && b == 2;

        private readonly Func<int, int, int, bool> _func3Args =
            (a, b, c) => a == 1 && b == 2 && c == 3;

        private readonly Func<int, int, int, int, bool> _func4Args =
            (a, b, c, d) => a == 1 && b == 2 && c == 3 && d == 4;

        private readonly Func<int, int, int, int, int, bool> _func5Args =
            (a, b, c, d, e) => a == 1 && b == 2 && c == 3 && d == 4 && e == 5;

        private readonly Func<(Action<int, int>, Func<bool>)> _action2Args =
            () =>
            {
                var called = false;
                return ((a, b) => called = a == 1 && b == 2, () => called);
            };

        private readonly Func<(Action<int, int, int>, Func<bool>)> _action3Args =
            () =>
            {
                var called = false;
                return ((a, b, c) => called = a == 1 && b == 2 && c == 3, () => called);
            };

        private readonly Func<(Action<int, int, int, int>, Func<bool>)> _action4Args =
            () =>
            {
                var called = false;
                return ((a, b, c, d) => called = a == 1 && b == 2 && c == 3 && d == 4, () => called);
            };

        private readonly Func<(Action<int, int, int, int, int>, Func<bool>)> _action5Args =
            () =>
            {
                var called = false;
                return ((a, b, c, d, e) => called = a == 1 && b == 2 && c == 3 && d == 4 && e == 5, () => called);
            };

        #region Compose Into Func Tests
        [Fact]
        public void IntoFunc()
        {
            bool not(bool x) => !x;

            Assert.False(_func2Args.Into(not)(1, 2));
            Assert.True(_func2Args.Into(not)(11, 22));

            Assert.False(_func3Args.Into(not)(1, 2, 3));
            Assert.True(_func3Args.Into(not)(11, 22, 33));

            Assert.False(_func4Args.Into(not)(1, 2, 3, 4));
            Assert.True(_func4Args.Into(not)(11, 22, 33, 44));

            Assert.False(_func5Args.Into(not)(1, 2, 3, 4, 5));
            Assert.True(_func5Args.Into(not)(11, 22, 33, 44, 55));
        }
        #endregion


        #region Compose Into Func Tests
        [Fact]
        public void IntoAction()
        {
            void canThrow(bool x)
            {
                if (x)
                {
                    throw new Exception();
                }
            }

            Assert.Throws<Exception>(() => _func2Args.Into(canThrow)(1, 2));
            _func2Args.Into(canThrow)(11, 22);

            Assert.Throws<Exception>(() => _func3Args.Into(canThrow)(1, 2, 3));
            _func3Args.Into(canThrow)(11, 22, 33);

            Assert.Throws<Exception>(() => _func4Args.Into(canThrow)(1, 2, 3, 4));
            _func4Args.Into(canThrow)(11, 22, 33, 44);

            Assert.Throws<Exception>(() => _func5Args.Into(canThrow)(1, 2, 3, 4, 5));
            _func5Args.Into(canThrow)(11, 22, 33, 44, 55);
        }
        #endregion

        #region #region Reverse Args
        [Fact]
        public void ReverseArgs()
        {
            Assert.True(_func2Args.ReverseArgs()(2, 1));
            Assert.True(_func3Args.ReverseArgs()(3, 2, 1));
            Assert.True(_func4Args.ReverseArgs()(4, 3, 2, 1));
            Assert.True(_func5Args.ReverseArgs()(5, 4, 3, 2, 1));
        }
        #endregion

        #region Func Curreny and DeCurry Tests
        [Fact]
        public void CurryFunc()
        {
            Assert.True(_func2Args.Curry()(1)(2));
            Assert.False(_func2Args.Curry()(11)(22));

            Assert.True(_func3Args.Curry()(1)(2)(3));
            Assert.False(_func3Args.Curry()(11)(22)(33));

            Assert.True(_func4Args.Curry()(1)(2)(3)(4));
            Assert.False(_func4Args.Curry()(11)(22)(33)(44));

            Assert.True(_func5Args.Curry()(1)(2)(3)(4)(5));
            Assert.False(_func5Args.Curry()(11)(22)(33)(44)(55));
        }

        [Fact]
        public void DeCurryFunc()
        {
            Assert.True(_func2Args.Curry().DeCurry()(1, 2));
            Assert.False(_func2Args.Curry().DeCurry()(11, 22));

            Assert.True(_func3Args.Curry().DeCurry()(1, 2, 3));
            Assert.False(_func3Args.Curry().DeCurry()(11, 22, 33));

            Assert.True(_func4Args.Curry().DeCurry()(1, 2, 3, 4));
            Assert.False(_func4Args.Curry().DeCurry()(11, 22, 33, 44));

            Assert.True(_func5Args.Curry()(1)(2)(3)(4)(5));
            Assert.False(_func5Args.Curry().DeCurry()(11, 22, 33, 44, 55));
        }
        #endregion

        #region Action Curry and DeCurry Tests
        [Theory]
        [InlineData(1, 2, true)]
        [InlineData(11, 22, false)]
        public void CurryAction2Args(int a, int b, bool expected)
        {
            var test = _action2Args();
            test.Item1.Curry()(a)(b);
            Assert.Equal(expected, test.Item2());
        }
        
        [Theory]
        [InlineData(1, 2, true)]
        [InlineData(11, 22, false)]
        public void DeCurryAction2Args(int a, int b, bool expected)
        {
            var test = _action2Args();
            test.Item1.Curry().DeCurry()(a, b);
            Assert.Equal(expected, test.Item2());
        }

        [Theory]
        [InlineData(1, 2, 3, true)]
        [InlineData(11, 22, 33, false)]
        public void CurryAction3Args(int a, int b, int c, bool expected)
        {
            var test = _action3Args();
            test.Item1.Curry()(a)(b)(c);
            Assert.Equal(expected, test.Item2());
        }
        
        [Theory]
        [InlineData(1, 2, 3, true)]
        [InlineData(11, 22, 33, false)]
        public void DeCurryAction3Args(int a, int b, int c, bool expected)
        {
            var test = _action3Args();
            test.Item1.Curry().DeCurry()(a, b, c);
            Assert.Equal(expected, test.Item2());
        }

        [Theory]
        [InlineData(1, 2, 3, 4, true)]
        [InlineData(11, 22, 33, 44, false)]
        public void CurryAction4Args(int a, int b, int c, int d, bool expected)
        {
            var test = _action4Args();
            test.Item1.Curry()(a)(b)(c)(d);
            Assert.Equal(expected, test.Item2());
        }

        [Theory]
        [InlineData(1, 2, 3, 4, true)]
        [InlineData(11, 22, 33, 44, false)]
        public void DeCurryAction4Args(int a, int b, int c, int d, bool expected)
        {
            var test = _action4Args();
            test.Item1.Curry().DeCurry()(a, b, c, d);
            Assert.Equal(expected, test.Item2());
        }

        [Theory]
        [InlineData(1, 2, 3, 4, 5, true)]
        [InlineData(11, 22, 33, 44, 55, false)]
        public void CurryAction5Args(int a, int b, int c, int d, int e, bool expected)
        {
            var test = _action5Args();
            test.Item1.Curry()(a)(b)(c)(d)(e);
            Assert.Equal(expected, test.Item2());
        }

        [Theory]
        [InlineData(1, 2, 3, 4, 5, true)]
        [InlineData(11, 22, 33, 44, 55, false)]
        public void DeCurryAction5Args(int a, int b, int c, int d, int e, bool expected)
        {
            var test = _action5Args();
            test.Item1.Curry().DeCurry()(a, b, c, d, e);
            Assert.Equal(expected, test.Item2());
        }
        #endregion
    }
}
