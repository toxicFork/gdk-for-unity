using System.Collections.Generic;

namespace Improbable.Gdk.Core
{
    internal class DictionaryPool<TKey, TValue>
    {
        private readonly Stack<Dictionary<TKey, TValue>> pool = new Stack<Dictionary<TKey, TValue>>();

        private int maxBufferLength;
        private int minBufferLength;
        private int maxPoolSize;

        public DictionaryPool(int maxPoolSize)
        {
            this.maxPoolSize = maxPoolSize;
        }

        public Dictionary<TKey, TValue> Rent()
        {
            if (pool.Count == 0)
            {
                return new Dictionary<TKey, TValue>();
            }

            return pool.Pop();
        }

        public void Return(Dictionary<TKey, TValue> dictionary)
        {
            if (pool.Count >= maxPoolSize)
            {
                return;
            }

            dictionary.Clear();
            pool.Push(dictionary);
        }
    }
}
