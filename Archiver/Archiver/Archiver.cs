using System;
using System.IO;
using System.Linq;
using Archiver.Interfaces;

namespace Archiver.Archiver
{
    public static class Archiver
    {
        public static void Run(IEncoder encoder)
        {
            while (true)
            {
                Console.WriteLine("Please, enter parameter: \n1: Encode \n2: Decode");
                var input = Console.ReadKey();
                Console.WriteLine();
                if (input.KeyChar.Equals('1'))
                {
                    RunEncoder(encoder.Encode);
                    break;
                }
                if (input.KeyChar.Equals('2'))
                {
                    RunEncoder(encoder.Decode);
                    break;
                }
                Console.WriteLine("Invalid parameter!");
            }
        }

        private static void RunEncoder(Action<Stream, Stream> encoderAction)
        {
            var (inStream, outStream) = GetStreams();
            encoderAction(inStream, outStream);
            inStream.Close();
            outStream.Close();
        }

        private static (Stream, Stream) GetStreams()
        {
            while (true)
            {
                var input = Console.ReadLine()
                                   .Trim()
                                   .Split()
                                   .ToList();
                if (input.Count == 2)
                {
                    var str = input[0].Split('\\', '/');
                    var resultPath = string.Join('\\', str.Take(str.Length - 1).Append(input[1]));
                    try
                    {
                        var inStream = new FileStream($"{input[0]}", FileMode.Open, FileAccess.Read);
                        var outStream = new FileStream($"{resultPath}", FileMode.Create, FileAccess.Write);
                        return (inStream, outStream);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        throw;
                    }
                }
            }
        }
    }
}
