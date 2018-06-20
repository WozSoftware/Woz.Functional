namespace Woz.Functional
{
    public static class Predicates
    {
        public static bool AlwaysTrue<T>(T value) => true;
        public static bool AlwaysFalse<T>(T value) => false;
    }
}
