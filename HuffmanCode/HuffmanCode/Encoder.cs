using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using HuffmanCode.DataStructures;
using HuffmanCode.Interfaces;

namespace HuffmanCode.HuffmanCode
{
    public class Encoder : IEncoder
    {
        public void Encode(Stream inStream, Stream outStream)
        {
            var frequencyDict = CompilingFrequencyDict(inStream);
            var binaryTree = BinaryTreeCreation(frequencyDict);
            var codes = GetSymbolsWithCodes(binaryTree);
            if (EncodeFile(inStream, outStream, binaryTree, codes))
            {
                Console.WriteLine("All done!");
            }
        }

        public void Decode(Stream inStream, Stream outStream)
        {
            var binaryTree = RecoveryBinaryTree(inStream);
            var codes = GetCodesWithSymbols(binaryTree);
            if (DecodeFile(inStream, outStream, codes))
            {
                Console.WriteLine("All done!");
            }
        }

        private Dictionary<char, int> CompilingFrequencyDict(Stream stream)
        {
            stream.Seek(0, SeekOrigin.Begin);
            var frequencyDict = new Dictionary<char, int>();
            while (stream.Position < stream.Length)
            {
                var readByte = (char) stream.ReadByte();
                if (!frequencyDict.ContainsKey(readByte))
                {
                    frequencyDict[readByte] = 0;
                }
                frequencyDict[readByte]++;
            }
            return frequencyDict;
        }

        private IBinaryTree<char> RecoveryBinaryTree(Stream readStream)
        {
            readStream.Seek(1, SeekOrigin.Begin);
            var formatter = new BinaryFormatter();
            var bufLength = new byte[8];
            readStream.Read(bufLength);
            var length = BitConverter.ToInt64(bufLength);
            var buf = new byte[length];
            readStream.Read(buf);
            var memoryStream = new MemoryStream(buf);
            var binaryTree = formatter.Deserialize(memoryStream) as BinaryTree<char>;

            return binaryTree;
        }

        private IBinaryTree<char> BinaryTreeCreation(Dictionary<char, int> frequencyDict)
        {
            if (frequencyDict.Count == 1)
            {
                var tree = new BinaryTree<char>(frequencyDict.Keys.FirstOrDefault());
                tree.Merge(new BinaryTree<char>());
                return tree;
            }
            var priorityQueue = new PriorityQueue<IBinaryTree<char>>();
            foreach (var (symbol, frequency) in frequencyDict)
            {
                var tree = new BinaryTree<char>(symbol);
                priorityQueue.Add(tree, frequency);
            }
            while (priorityQueue.Count > 1)
            {
                var (tree, frequency) = priorityQueue.ExtractMin();
                var (otherTree, otherFrequency) = priorityQueue.ExtractMin();
                tree.Merge(otherTree);
                priorityQueue.Add(tree, frequency + otherFrequency);
            }
            return priorityQueue.Count > 0
                       ? priorityQueue.ExtractMin().Item1
                       : null;
        }

        private Dictionary<char, string> GetSymbolsWithCodes(IBinaryTree<char> binaryTree)
        {
            var codes = new Dictionary<char, string>();
            foreach (var (symbol, code) in Search(binaryTree.Root, new LinkedList<char>()))
            {
                codes[symbol] = code;
            }

            return codes;
        }

        private IReadOnlyDictionary<string, char> GetCodesWithSymbols(IBinaryTree<char> binaryTree)
        {
            var codes = new Dictionary<string, char>();
            foreach (var (symbol, code) in Search(binaryTree.Root, new LinkedList<char>()))
            {
                codes[code] = symbol;
            }

            return codes;
        }

        private bool EncodeFile(Stream inStream,
                                Stream outStream,
                                IBinaryTree<char> binaryTree,
                                Dictionary<char, string> codes)
        {
            inStream.Seek(0, SeekOrigin.Begin);
            if (binaryTree.GetType().GetCustomAttribute<SerializableAttribute>() == null)
            {
                return false;
            }
            outStream.WriteByte(0); //Number of bits added at the end
            WriteBinaryHeap(outStream, binaryTree);
            CompressAndWrite(inStream, outStream, codes);

            return true;
        }

        private void WriteBinaryHeap(Stream outStream, IBinaryTree<char> binaryTree)
        {
            var start = outStream.Length;
            outStream.Write(new byte[8]); //Binary heap length
            var formatter = new BinaryFormatter();
            formatter.Serialize(outStream, binaryTree);
            outStream.Position = start;
            outStream.Write(BitConverter.GetBytes(outStream.Length - start - 8));
            outStream.Position = outStream.Length;
        }

        private void CompressAndWrite(Stream inStream, Stream outStream, IReadOnlyDictionary<char, string> codes)
        {
            var buffer = new List<char>();
            while (inStream.Position < inStream.Length)
            {
                if (buffer.Count > 7)
                {
                    buffer = WriteByte(outStream, buffer);
                }
                else
                {
                    var readByte = (char) inStream.ReadByte();
                    Console.Write($"\r{inStream.Position / inStream.Length * 100:f1}%");
                    buffer.AddRange(codes[readByte].ToCharArray());
                }
            }
            if (buffer.Count > 7)
            {
                buffer = WriteByte(outStream, buffer);
            }
            if (buffer.Count > 0)
            {
                WriteLastByte(outStream, buffer);
            }
        }

        private static List<char> WriteByte(Stream outStream, List<char> buffer)
        {
            var current = Convert.ToByte(string.Join("", buffer.Take(8)), 2);
            outStream.WriteByte(current);
            buffer = buffer.Skip(8).ToList();
            return buffer;
        }

        private static void WriteLastByte(Stream outStream, List<char> buffer)
        {
            var count = 8 - buffer.Count;
            buffer.AddRange(Enumerable.Repeat('0', count));
            var current = Convert.ToByte(string.Join("", buffer), 2);
            outStream.WriteByte(current);
            outStream.Seek(0, SeekOrigin.Begin);
            outStream.WriteByte((byte) count);
        }

        private IEnumerable<Tuple<char, string>> Search(TreeNode<char> current, LinkedList<char> list)
        {
            if (!current.Value.Equals('\0'))
            {
                yield return Tuple.Create(current.Value, string.Join("", list));
                yield break;
            }
            if (current.Left != null)
            {
                list.AddLast('0');
                foreach (var tuple in Search(current.Left, list))
                {
                    yield return tuple;
                }
                list.RemoveLast();
            }
            if (current.Right != null)
            {
                list.AddLast('1');
                foreach (var tuple in Search(current.Right, list))
                {
                    yield return tuple;
                }
                list.RemoveLast();
            }
        }

        private bool DecodeFile(Stream read, Stream outStream, IReadOnlyDictionary<string, char> codes)
        {
            read.Seek(0, SeekOrigin.Begin);
            var queue = new Queue<char>();
            var count = read.ReadByte();
            var buf = new byte[8];
            read.Read(buf);
            read.Seek(BitConverter.ToInt64(buf), SeekOrigin.Current);

            var str = new StringBuilder();
            while (read.Position < read.Length - 1)
            {
                foreach (var er in Convert.ToString(read.ReadByte(), 2).PadLeft(8, '0'))
                {
                    queue.Enqueue(er);
                }
                WriteBytes(codes, queue, str, outStream);
            }
            var s = Convert.ToString(read.ReadByte(), 2).PadLeft(8, '0');
            for (var i = 0; i < s.Length - count; i++)
            {
                queue.Enqueue(s[i]);
            }
            WriteBytes(codes, queue, str, outStream);

            return true;
        }

        private static void WriteBytes(IReadOnlyDictionary<string, char> codes,
                                       Queue<char> queue,
                                       StringBuilder str,
                                       Stream outStream)
        {
            while (queue.Count > 0)
            {
                str.Append(queue.Dequeue());
                if (codes.ContainsKey(str.ToString()))
                {
                    outStream.WriteByte((byte) codes[str.ToString()]);
                    str.Clear();
                }
            }
        }
    }
}
