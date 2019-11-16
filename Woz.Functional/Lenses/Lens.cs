using System;

namespace Woz.Functional.Lenses
{
    public static class Lens
    {
        public static Lens<TType, TValue> Create<TType, TValue>(
            Func<TType, TValue> get, Func<TValue, Func<TType, TType>> set) 
            => new Lens<TType, TValue>(get, set);

        public static TValue Get<TType, TValue>(
            this TType instance, Lens<TType, TValue> lens) 
            => lens.Get(instance);

        public static TType Set<TType, TValue>(
            this TType instance, Lens<TType, TValue> lens, TValue value) 
            => lens.Set(value)(instance);
    }

    public class Lens<TType, TValue>
    {
        internal Lens(
            Func<TType, TValue> get, Func<TValue, Func<TType, TType>> set)
        {
            Get = get;
            Set = set;
        }

        public Func<TType, TValue> Get { get; }
        public Func<TValue, Func<TType, TType>> Set { get; }

        private Func<TType, TType> Apply(Func<TValue, TValue> updater) 
            => root => Set(updater(Get(root)))(root);

        public Lens<TType, TChildValue> Into<TChildValue>(
            Lens<TValue, TChildValue> childLens) 
            => new Lens<TType, TChildValue>(
                root => childLens.Get(Get(root)),
                childValue => Apply(childLens.Set(childValue)));
    }
}
