using System;
using System.Collections.Generic;
using NUnit.Framework;

[assembly: IndexesStartAtOne]

namespace IndexesStartAt.Tests
{
    public static class UnitTests
    {
        private static readonly int[] Values = { 4, 8, 15, 16, 23, 42 };

        [Test]
        public static void ShouldIndexWithConstants()
        {
            Assert.AreEqual(Values[1], 4);
        }

        [Test]
        public static void ShouldThrowWhenTakingZerothArgument()
        {
            Assert.Throws<IndexOutOfRangeException>(() =>
            {
                int _ = Values[0];
            });
        }

        [Test]
        public static void ShouldIndexWithVariables()
        {
            for (int i = 1; i <= Values.Length; i++)
            {
                Assert.DoesNotThrow(() =>
                {
                    int _ = Values[i];
                });
            }
        }

        [Test]
        public static void ShouldIndexMatrices()
        {
            int[,] m =
            {
                { 1, 2, 3 },
                { 4, 5, 6 },
                { 7, 8, 9 }
            };

            Assert.AreEqual(m[1, 1], 1);
            Assert.AreEqual(m[3, 3], 9);
        }

        [Test]
        public static void ShouldNotChangeNonIntegers()
        {
            string GetKey() => "Test2";

            var test = new Dictionary<string, string>
            {
                { "Test1", "It works." },
                { "Test2", "It works with non-literals as well." }
            };

            Assert.AreEqual(test["Test1"], "It works.");
            Assert.AreEqual(test[GetKey()], "It works with non-literals as well.");
        }
    }
}
