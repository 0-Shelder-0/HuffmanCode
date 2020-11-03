using System.IO;

namespace HuffmanCode.Interfaces
{
    public interface IEncoder
    {
        void Encode(Stream inStream, Stream outStream);
        void Decode(Stream inStream, Stream outStream);
    }
}
