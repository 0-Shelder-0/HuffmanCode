using System;

namespace HuffmanCode.DataStructures
{
    [Serializable]
    public class TreeNode<T>
    {
        public T Value { get; }
        public TreeNode<T> Left, Right;

        public TreeNode() : this(default) { }

        public TreeNode(T value)
        {
            Value = value;
        }
    }
}
