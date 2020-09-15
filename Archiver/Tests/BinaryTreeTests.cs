using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Archiver.DataStructures;

namespace Archiver.Tests
{
    [TestFixture]
    public class BinaryTreeTests
    {
        private static void Test(string input, string expected)
        {
            var trees = input.Split(" ")
                             .Where(str => str.Length > 0)
                             .Select(priority => new BinaryTree<char>(new TreeNode<char>('\0', int.Parse(priority))))
                             .ToList();

            var result = trees.FirstOrDefault();
            foreach (var tree in trees.Skip(1))
            {
                result.Merge(tree);
            }

            var a = GetItems(result.Root).ToList();
        }

        private static IEnumerable<TreeNode<char>> GetItems(TreeNode<char> treeNode)
        {
            if (treeNode == null)
                yield break;
            foreach (var node in GetItems(treeNode.Left))
                yield return node;
            yield return treeNode;
            foreach (var node in GetItems(treeNode.Right))
                yield return node;
        }

        // [TestCase("1 2", "")]
        public static void RunTests(string input, string expected)
        {
            Test(input, expected);
        }
    }
}
