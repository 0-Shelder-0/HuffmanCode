namespace Archiver.DataStructures
{
    public class TreeNode<T>
    {
        public T Value { get; set; }
        public int Priority { get; set; }
        public TreeNode<T> Left, Right;
        
        public TreeNode() { }
        
        public TreeNode(T value, int priority = 0)
        {
            Value = value;
            Priority = priority;
        }
    }
}
