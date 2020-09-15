namespace Archiver.DataStructures
{
    public class QueueNode<TKey>
    {
        public TKey Key { get; }
        public int Value { get; set; }

        public QueueNode(TKey key, int value = 0)
        {
            Key = key;
            Value = value;
        }
    }
}
