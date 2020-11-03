using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace HuffmanCode.Tests
{
    [TestFixture]
    public class HuffmanCodeTests
    {
        private static void Test(string input)
        {
            var encoder = new HuffmanCode.Encoder();
            var inStream = new MemoryStream(Encoding.Default.GetBytes(input));
            var outStream = new MemoryStream();
            var resultStream = new MemoryStream();
            encoder.Encode(inStream, outStream);
            encoder.Decode(outStream, resultStream);
            var result = Encoding.Default.GetString(resultStream.GetBuffer()
                                                                .Take(input.Length)
                                                                .ToArray());

            Assert.AreEqual(input, result);
        }

        [TestCase("aaa")]
        [TestCase("aabbb")]
        [TestCase("aabbbcccc")]
        [TestCase("beep boop beer!")]
        [TestCase("aaaa b ccc ddddddd ee ffff")]
        public static void RunTests(string input)
        {
            Test(input);
        }


        [TestCase("BigTest.txt")]
        public static void RunBigTest(string fileName)
        {
            var splitPath = Directory.GetCurrentDirectory().Split('\\');
            var path = string.Join('\\', splitPath.Take(splitPath.Length - 3));
            path += $"\\TestFiles\\{fileName}";
            BigTest(path);
        }

        private static void BigTest(string path)
        {
            var encoder = new HuffmanCode.Encoder();
            var inStream = new FileStream(path, FileMode.Open, FileAccess.Read);
            var bytes = new byte[inStream.Length];
            inStream.Read(bytes);
            inStream.Seek(0, SeekOrigin.Begin);
            var file = Encoding.Default.GetString(bytes);
            var outStream = new MemoryStream();
            var resultStream = new MemoryStream();
            encoder.Encode(inStream, outStream);
            encoder.Decode(outStream, resultStream);
            var result = Encoding.Default.GetString(resultStream.GetBuffer()
                                                                .Take((int) inStream.Length)
                                                                .ToArray());

            Assert.AreEqual(file, result);
        }
    }
}
