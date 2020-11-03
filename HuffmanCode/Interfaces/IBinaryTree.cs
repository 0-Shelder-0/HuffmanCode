using HuffmanCode.DataStructures;

namespace HuffmanCode.Interfaces
{
    public interface IBinaryTree<T>
    {
        TreeNode<T> Root { get; }
        void Merge(IBinaryTree<T> otherTree);
    }
}
