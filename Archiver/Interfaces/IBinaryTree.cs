using Archiver.DataStructures;

namespace Archiver.Interfaces
{
    public interface IBinaryTree<T>
    {
        TreeNode<T> Root { get; }
        void Merge(IBinaryTree<T> otherTree);
    }
}
