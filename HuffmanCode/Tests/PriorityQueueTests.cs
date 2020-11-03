using System;
using System.Collections.Generic;
using System.Linq;
using HuffmanCode.DataStructures;
using NUnit.Framework;

namespace HuffmanCode.Tests
{
    [TestFixture]
    public class PriorityQueueTests
    {
        private static void Test(string input, string expected)
        {
            var priorityQueue = new PriorityQueue<char>();
            var expectedValues = expected.Split(' ')
                                         .Where(str => str.Length > 0)
                                         .Select(int.Parse)
                                         .ToList();
            var expectedKeys = expectedValues.Select(value => (char) value)
                                             .ToList();

            foreach (var value in expectedValues)
            {
                priorityQueue.Add((char) value, value);
            }

            var keys = new List<char>();
            var values = new List<int>();
            foreach (var e in expectedValues)
            {
                var (key, value) = priorityQueue.ExtractMin();
                keys.Add(key);
                values.Add(value);
            }

            Assert.AreEqual(keys, expectedKeys);
            Assert.AreEqual(values, expectedValues);
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
