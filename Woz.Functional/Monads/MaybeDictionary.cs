using System.Collections.Generic;

namespace Woz.Functional.Monads
{
    public static class MaybeDictionary
    {
        public static Maybe<TValue> MaybeFind<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
            where TKey : notnull 
            => dictionary.TryGetValue(key, out TValue value) ? value.ToMaybe() : Maybe<TValue>.None;
    }
}
