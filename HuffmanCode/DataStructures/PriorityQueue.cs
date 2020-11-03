using System;
using System.Collections.Generic;
using HuffmanCode.Interfaces;

namespace HuffmanCode.DataStructures
{
    public class PriorityQueue<TKey> : IPriorityQueue<TKey>
    {
        private List<QueueNode<TKey>> _tree;

        public int Count { get; private set; }

        public void Add(TKey key, int value)
        {
            var queueNode = new QueueNode<TKey>(key, value);
            if (_tree == null)
            {
                _tree = new List<QueueNode<TKey>> {queueNode};
            }
            else
            {
                _tree.Add(queueNode);
                HeapifyUp();
            }
            Count++;
        }

        public void Merge(IPriorityQueue<TKey> otherQueue)
        {
            if (Count >= otherQueue.Count)
            {
                Merge(this, otherQueue);
            }
            else
            {
                Merge(otherQueue, this);
            }
        }

        private static void Merge(IPriorityQueue<TKey> queue,
                                  IPriorityQueue<TKey> otherQueue)
        {
            while (otherQueue.Count > 0)
            {
                var (key, value) = otherQueue.ExtractMin();
                queue.Add(key, value);
            }
        }

        public Tuple<TKey, int> ExtractMin()
        {
            if (Count == 0)
            {
                throw new InvalidOperationException();
            }
            var min = _tree[0];
            _tree[0] = _tree[^1];
            _tree.RemoveAt(_tree.Count - 1);
            HeapifyDown();
            Count--;

            return Tuple.Create(min.Key, min.Value);
        }

        private void HeapifyDown()
        {
            var current = 0;

            while (current * 2 + 1 < _tree.Count)
            {
                var left = 2 * current + 1;
                var right = 2 * current + 2;
                var minChild = GetMinChild(left, right);

                if (_tree[current].Value > _tree[minChild].Value)
                {
                    (_tree[current], _tree[minChild]) = (_tree[minChild], _tree[current]);
                    current = minChild;
                }
                else
                {
                    break;
                }
            }
        }

        private int GetMinChild(int left, int right)
        {
            if (left == _tree.Count - 1 || _tree[left].Value <= _tree[right].Value)
            {
                return left;
            }
            return right;
        }

        private void HeapifyUp()
        {
            var index = _tree.Count - 1;
            var parent = (index - 1) / 2;

            while (_tree[parent].Value > _tree[index].Value)
            {
                (_tree[index], _tree[parent]) = (_tree[parent], _tree[index]);
                index = parent;
                parent = (parent - 1) / 2;
            }
        }
    }
}
