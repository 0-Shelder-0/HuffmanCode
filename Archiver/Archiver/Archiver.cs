using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Archiver.DataStructures;
using Archiver.Interfaces;

namespace Archiver.Archiver
{
    public class Archiver
    {
        private static string Path;
        private static Dictionary<char, int> Dict;
        private static Dictionary<char, string> Codes;
        private static IPriorityQueue<IBinaryTree<char>> PriorityQueue;
        private static IBinaryTree<char> BinaryTree;

        public static void Run()
        {
            InputPath();
            CompilingFrequencyDict();
            BinaryTreeCreation();
            GetSymbolCods();
            EncodeFile();
        }

        private static void InputPath()
        {
            Console.WriteLine("Enter path to file:");
            // return Console.ReadLine();
            Path = @"C:\Users\Vlad\Desktop\1.txt";
        }

        private static void CompilingFrequencyDict()
        {
            var stream = new FileStream(Path, FileMode.Open, FileAccess.Read);
            Dict = new Dictionary<char, int>();

            while (stream.CanRead)
            {
                var b = (char) stream.ReadByte();
                if (b == char.MaxValue) break;
                if (!Dict.ContainsKey(b))
                {
                    Dict[b] = 0;
                }
                Dict[b]++;
            }
            stream.Close();
        }

        private static void BinaryTreeCreation()
        {
            PriorityQueue = new PriorityQueue<IBinaryTree<char>>();

            foreach (var (symbol, frequency) in Dict)
            {
                var tree = new BinaryTree<char>(symbol, frequency);
                PriorityQueue.Add(tree, frequency);
            }

            while (PriorityQueue.Count > 1)
            {
                var (tree, frequency) = PriorityQueue.ExtractMin();
                var (otherTree, otherFrequency) = PriorityQueue.ExtractMin();
                tree.Merge(otherTree);
                PriorityQueue.Add(tree, frequency + otherFrequency);
            }
            if (PriorityQueue.Count > 0)
            {
                BinaryTree = PriorityQueue.ExtractMin().Item1;
            }
        }

        private static void GetSymbolCods()
        {
            Codes = new Dictionary<char, string>();
            Search(BinaryTree.Root, new LinkedList<char>());
        }

        private static void Search(TreeNode<char> current, LinkedList<char> list)
        {
            if (!current.Value.Equals('\0'))
            {
                Codes[current.Value] = string.Join('\0', list);
            }
            if (current.Left != null)
            {
                list.AddLast('0');
                Search(current.Left, list);
                list.RemoveLast();
            }
            if (current.Right != null)
            {
                list.AddLast('1');
                Search(current.Right, list);
                list.RemoveLast();
            }
        }

        private static void EncodeFile()
        {
            
        }
    }
}
