namespace Woz.Functional
{
    // TODO: Not happy with the class name. Wait to see what bundles in here

    public static class FreeFunctions
    {
        // NOTE: These are intended for static using

        public static T Identity<T>(T id) => id;

        public static TL Left<TL, TR>(TL left, TR right) => left;
        public static TR Right<TL, TR>(TL left, TR right) => right;

    }
}
