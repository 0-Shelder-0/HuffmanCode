using System;
using System.Collections.Generic;
using Archiver.Interfaces;

namespace Archiver.DataStructures
{
    public class PriorityQueue<TKey> : IPriorityQueue<TKey>
    {
        private List<QueueNode<TKey>> _tree;

        public void Add(TKey key, int value)
        {
            if (_tree == null)
            {
                _tree = new List<QueueNode<TKey>> {new QueueNode<TKey>(key, value)};
            }
            else
            {
                _tree.Add(new QueueNode<TKey>(key, value));
                HeapifyUp();
            }
        }

        public Tuple<TKey, int> ExtractMin()
        {
            var min = _tree[0];
            _tree[0] = _tree[^1];
            _tree.RemoveAt(_tree.Count - 1);
            HeapifyDown();

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
                    (_tree[current].Value, _tree[minChild].Value) =
                        (_tree[minChild].Value, _tree[current].Value);
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

            while (index > 0 && _tree[parent].Value > _tree[index].Value)
            {
                (_tree[index], _tree[parent]) = (_tree[parent], _tree[index]);
                index = parent;
                parent = (parent - 1) / 2;
            }
        }
    }
}
