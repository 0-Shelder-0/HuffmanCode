using System;
using Archiver.Interfaces;

namespace Archiver.DataStructures
{
    [Serializable]
    public class BinaryTree<T> : IBinaryTree<T>
    {
        public TreeNode<T> Root { get; private set; }

        public BinaryTree() : this(default) { }

        public BinaryTree(T value)
        {
            Root = new TreeNode<T>(value);
        }

        public void Merge(IBinaryTree<T> otherTree)
        {
            Root = new TreeNode<T>
            {
                Left = Root,
                Right = otherTree.Root
            };
        }
    }
}
