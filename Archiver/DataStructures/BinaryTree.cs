using Archiver.Interfaces;

namespace Archiver.DataStructures
{
    public class BinaryTree<T> : IBinaryTree<T>
    {
        public TreeNode<T> Root { get; private set; }

        public BinaryTree(T value, int priority)
        {
            Root = new TreeNode<T>(value, priority);
        }

        public void Merge(IBinaryTree<T> otherTree)
        {
            Root = new TreeNode<T>
            {
                Value = default,
                Priority = Root.Priority + otherTree.Root.Priority,
                Left = Root,
                Right = otherTree.Root
            };
        }
        
        
    }
}
