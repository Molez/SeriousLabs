using NUnit.Framework;
using Q1App;

namespace Q1Test
{
    //Not expansive testing but proves the basic assumptions
    [TestFixture]
    public class Q1Tests
    {
        [Test]
        public void AscendingOrderedMatches()
        {
            var inputArray = new int[] { 1, 3, 5, 7, 9, 11, 13, 15 };

            //I could have done this with [TestCase] too. Maybe a bit cleaner that way and easier to track down failures.
            var result = Q1.Func(inputArray, 11, 0, inputArray.Length - 1);
            Assert.AreEqual(5, result);

            result = Q1.Func(inputArray, 13, 0, inputArray.Length - 1);
            Assert.AreEqual(6, result);

            result = Q1.Func(inputArray, 3, 0, inputArray.Length - 1);
            Assert.AreEqual(1, result);

            //Test the edges too
            result = Q1.Func(inputArray, 1, 0, inputArray.Length - 1);
            Assert.AreEqual(0, result);

            result = Q1.Func(inputArray, 15, 0, inputArray.Length - 1);
            Assert.AreEqual(7, result);
        }

        [Test]
        public void AscendingOrderedMatchesNegatives()
        {
            var inputArray = new int[] { -8, -7, -2, 3, 8 };

            var result = Q1.Func(inputArray, -7, 0, inputArray.Length - 1);
            Assert.AreEqual(1, result);

            result = Q1.Func(inputArray, 8, 0, inputArray.Length - 1);
            Assert.AreEqual(4, result);

            result = Q1.Func(inputArray, 8, 0, inputArray.Length - 1);
            Assert.AreEqual(4, result);
        }

        [Test]
        public void AscendingOrderedNoMatchInbounds()
        {
            var inputArray = new int[] { 1, 3, 5, 7, 9, 11, 13, 15 };
            var result = Q1.Func(inputArray, 6, 0, inputArray.Length - 1);
            Assert.AreEqual(-1, result);
        }

        [Test]
        public void AscendingOrderedNoMatchOutbounds()
        {
            var inputArray = new int[] { 1, 3, 5, 7, 9, 11, 13, 15 };
            var result = Q1.Func(inputArray, 20, 0, inputArray.Length - 1);
            Assert.AreEqual(-1, result);
        }

        [Test]
        public void DescendingOrderedMatch()
        {
            //We can find a match if the number is the middle so we get it on the first guess.
            //This isnt to prove it works just that it can look like it works in some cases
            var inputArray = new int[] { 17, 15, 13, 11, 9, 7, 5, 3, 1 };
            var result = Q1.Func(inputArray, 9, 0, inputArray.Length - 1);
            Assert.AreEqual(4, result);
        }

        [Test]
        public void DescendingOrderedNoMatchWhenExists()
        {
            //We cannot find anythign on either side of middle
            var inputArray = new int[] { 15, 13, 11, 9, 7, 5, 3, 1 };
            var result = Q1.Func(inputArray, 13, 0, inputArray.Length - 1);
            Assert.AreEqual(-1, result);

            result = Q1.Func(inputArray, 5, 0, inputArray.Length - 1);
            Assert.AreEqual(-1, result);
        }

        [Test]
        public void UnorderedNoMatchWhenExists()
        {
            //We may or maynot find things if its unordered
            var inputArray = new int[] { 6, 3, 10, 8, 3, 67, 29 };
            //Cant find 10
            var result = Q1.Func(inputArray, 10, 0, inputArray.Length - 1);
            Assert.AreEqual(-1, result);

            //Can find 67
            result = Q1.Func(inputArray, 67, 0, inputArray.Length - 1);
            Assert.AreEqual(5, result);

        }
    }
}
