using System;

namespace HuffmanCode.Interfaces
{
    public interface IPriorityQueue<TKey>
    {
        public int Count { get; }
        void Add(TKey key, int value);
        void Merge(IPriorityQueue<TKey> otherQueue);
        Tuple<TKey, int> ExtractMin();
    }
}
