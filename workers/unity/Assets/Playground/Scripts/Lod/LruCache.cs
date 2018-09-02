using System.Collections.Generic;

namespace Playground
{
    public class LruCache<TKey, TValue>
    {
        public readonly int Capacity;
        public int Size;
        private readonly Dictionary<TKey, CacheNode<TKey, TValue>> cache;
        private readonly CacheNode<TKey, TValue> head;
        private readonly CacheNode<TKey, TValue> tail;

        public LruCache(int capacity)
        {
            Capacity = capacity;
            cache = new Dictionary<TKey, CacheNode<TKey, TValue>>(capacity);

            Size = 0;
            head = new CacheNode<TKey, TValue>();
            tail = new CacheNode<TKey, TValue>();

            head.Prev = null;
            head.Next = tail;
            tail.Prev = head;
            tail.Next = null;
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            if (cache.TryGetValue(key, out var node))
            {
                MoveNodeToFront(node);
                value = node.Value;
                return true;
            }

            value = default(TValue);
            return false;
        }

        public void Set(TKey key, TValue value)
        {
            if (cache.ContainsKey(key))
            {
                var node = cache[key];
                node.Value = value;
                MoveNodeToFront(node);
            }
            else
            {
                var node = new CacheNode<TKey, TValue>
                {
                    Key = key,
                    Value = value
                };

                cache.Add(key, node);
                LinkAtFront(node);

                Size++;
                if (Capacity < Size)
                {
                    cache.Remove(tail.Prev.Key);
                    RemoveLastNode();
                    Size--;
                }
            }
        }


        private void LinkAtFront(CacheNode<TKey, TValue> node)
        {
            var headNext = head.Next;
            head.Next = node;
            node.Prev = head;
            node.Next = headNext;
            headNext.Prev = node;
        }


        private void RemoveLastNode()
        {
            var tailPrev = tail.Prev;
            var tailPrevPrev = tailPrev.Prev;
            tailPrevPrev.Next = tail;
            tail.Prev = tailPrevPrev;
        }


        private void MoveNodeToFront(CacheNode<TKey, TValue> node)
        {
            var prevNode = node.Prev;
            var nextNode = node.Next;

            prevNode.Next = nextNode;
            nextNode.Prev = prevNode;

            LinkAtFront(node);
        }
    }


    internal class CacheNode<TKey, TValue>
    {
        public TKey Key;
        public TValue Value;
        public CacheNode<TKey, TValue> Prev;
        public CacheNode<TKey, TValue> Next;
    }
}
