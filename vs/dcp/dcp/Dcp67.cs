using System;
using System.Collections.Generic;
using System.Text;

namespace dcp
{
    /*
     * This problem was asked by Google.

       Implement an LFU (Least Frequently Used) cache. It should be able to be initialized with a cache size n, 
       and contain the following methods:

       set(key, value): sets key to value. If there are already n items in the cache and we are 
       adding a new item, then it should also remove the least frequently used item. If there is a tie, 
       then the least recently used key should be removed.

       get(key): gets the value at key. If no such key exists, return null.
       Each operation should run in O(1) time.
     */

    public static class Dcp67
    {
        public static void Test()
        {
            var cache = new Lru<string, int?>(3);
            cache.Set("a", 1);
            cache.Set("a", 2);
            cache.Set("b", 3);
            cache.Set("c", 4);

            Console.WriteLine("a={0}, b={1}, c={2}", cache.Get("a"), cache.Get("b"), cache.Get("c"));
            cache.Set("d", 5);
            Console.WriteLine("a={0}, b={1}, c={2}, d={3}", cache.Get("a"), cache.Get("b"), cache.Get("c"), cache.Get("d"));
        }

        private class Lru<TKey, TValue>
        {
            private readonly LinkedList<(TKey key, TValue value)> _ll;
            private readonly Dictionary<TKey, LinkedListNode<(TKey key, TValue value)>> _ht;
            private readonly int _n;

            public Lru(int n)
            {
                _ht = new Dictionary<TKey, LinkedListNode<(TKey, TValue)>>(n);
                _ll = new LinkedList<(TKey, TValue)>();
                _n = n;
            }

            public void Set(TKey key, TValue value)
            {
                if (_ht.TryGetValue(key, out var existingNode))
                {
                    // modify existing - we're sure there will be no eviction, so returning early
                    existingNode.Value = (key, value);
                    // bring it to head as we just accessed it
                    _ll.Remove(existingNode);
                    _ll.AddFirst(existingNode);
                    return;
                }

                if (_ll.Count == _n)
                {
                    // evict, use backreference from linked list to hashtable
                    var lfuKey = _ll.Last.Value.key;
                    _ht.Remove(lfuKey);
                    _ll.RemoveLast();
                }

                var newNode = _ll.AddFirst((key, value));
                _ht.Add(key, newNode);
            }

            public TValue Get(TKey key)
            {
                if (_ht.TryGetValue(key, out var existingValue))
                {
                    _ll.Remove(existingValue);
                    _ll.AddFirst(existingValue);    // bring the value we just read to the head of the list
                    return existingValue.Value.value;
                }
                return default(TValue);
            }
        }
    }
}
