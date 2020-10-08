using Archiver.DataStructures;

namespace Archiver
{
    public class Program
    {
        public static void Main()
        {
            var encoder = new Encoder();
            Archiver.Archiver.Run(encoder);
        }
    }
}
