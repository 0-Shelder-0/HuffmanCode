using HuffmanCode.HuffmanCode;

namespace HuffmanCode
{
    public static class Program
    {
        public static void Main()
        {
            var encoder = new Encoder();
            HuffmanCode.HuffmanCode.Run(encoder);
        }
    }
}
