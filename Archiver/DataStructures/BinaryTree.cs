using Archiver.Interfaces;

namespace Archiver.DataStructures
{
    public class BinaryTree<T> : IBinaryTree<T>
    {
        public TreeNode<T> Root { get; private set; }

        public BinaryTree(TreeNode<T> root)
        {
            Root = root;
        }

        public void Merge(IBinaryTree<T> otherNode)
        {
            Root = new TreeNode<T>
            {
                Value = default,
                Priority = Root.Priority + otherNode.Root.Priority,
                Left = Root,
                Right = otherNode.Root
            };
        }

        public bool Contains(T value)
        {
            return Search(Root, value);
        }

        private bool Search(TreeNode<T> current, T value)
        {
            if (current.Left != null)
            {
                Search(current.Left, value);
            }
            else if (current.Right != null)
            {
                Search(current.Right, value);
            }
            return current.Value.Equals(value);
        }
    }
}
