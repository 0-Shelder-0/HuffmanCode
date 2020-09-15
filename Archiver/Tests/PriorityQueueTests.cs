using System.Linq;
using Archiver.DataStructures;
using NUnit.Framework;

namespace Archiver.Tests
{
    [TestFixture]
    public class PriorityQueueTests
    {
        private static void Test(string input, string expectedResult)
        {
            var priorityQueue = new PriorityQueue<char>();
            var inp = input.Split(' ')
                           .Where(str => str.Length > 0)
                           .ToList();

            foreach (var ch in inp)
            {
                priorityQueue.Add('\0', int.Parse(ch));
            }

            var result = string.Join(" ", inp.Select(x => priorityQueue.ExtractMin().Item2).ToList());

            Assert.AreEqual(result, expectedResult);
        }

        [TestCase("", "")]
        [TestCase("1", "1")]
        [TestCase("1 2", "1 2")]
        [TestCase("2 1", "1 2")]
        [TestCase("3 2 4", "2 3 4")]
        [TestCase("1 2 3 4 5", "1 2 3 4 5")]
        [TestCase("5 4 1 2 3", "1 2 3 4 5")]
        [TestCase("5 4 3 2 1", "1 2 3 4 5")]
        [TestCase("5 1 6 3 9 4 2 7 6", "1 2 3 4 5 6 6 7 9")]
        [TestCase("1 1 10 3 2 2 2 2 2 3 1 1", "1 1 1 1 2 2 2 2 2 3 3 10")]
        public static void RunTests(string input, string expected)
        {
            Test(input, expected);
        }
    }
}
