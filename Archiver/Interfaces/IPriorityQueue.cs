using System;

namespace Archiver.Interfaces
{
    public interface IPriorityQueue<TKey>
    {
        void Add(TKey key, int value);
        Tuple<TKey, int> ExtractMin();
    }
}
