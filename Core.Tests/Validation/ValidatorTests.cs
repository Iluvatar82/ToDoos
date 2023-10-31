using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Core.Validation.Tests
{
    [TestClass()]
    public class ValidatorTests
    {
        [TestMethod()]
        public void NotNullTest_Ok()
        {
            var testString = "a nice string";
            testString.NotNull();

            Assert.AreEqual(false, string.IsNullOrWhiteSpace(testString));
        }

        [TestMethod()]
        public void NotNullTest_IsNull_Should_Throw()
        {
            string? testString = null;
            Assert.ThrowsException<ArgumentNullException>(() => testString.NotNull());
        }

        [TestMethod()]
        public void NotNullOrEmptyTest_Ok()
        {
            var testString = "a nice string not null, nor is it empty";
            testString.NotNull();

            Assert.AreEqual(false, string.IsNullOrWhiteSpace(testString));
        }

        [TestMethod()]
        public void NotNullOrEmptyTest_IsNull_Should_Throw()
        {
            string? testString = null;
            Assert.ThrowsException<ArgumentNullException>(() => testString.NotNullOrEmpty());
        }

        [TestMethod()]
        public void NotNullOrEmptyTest_IsEmpty_Should_Throw()
        {
            var testString = string.Empty;
            Assert.ThrowsException<ArgumentNullException>(() => testString.NotNullOrEmpty());
        }

        [TestMethod()]
        public void SatisfiesTest_Value_Ok()
        {
            Func<int, int, bool> comparisonFunc = (int number, int biggerThan) => number > biggerThan;
            
            var number = 11;
            var compareValue = 10;

            number.Satisfies(n => comparisonFunc(n, compareValue));

            Assert.AreEqual(true, comparisonFunc(number, compareValue));
        }

        [TestMethod()]
        public void SatisfiesTest_AllElements_Ok()
        {
            Func<int, int, bool> comparisonFunc = (int number, int biggerThan) => number > biggerThan;

            var compareValue = 10;

            var array = new[] { 12, 15, 18, 11 };
            Array.ForEach(array, (int num) => num.Satisfies(nintern => comparisonFunc(nintern, compareValue)));

            Assert.AreEqual(true, array.All(num => num > compareValue));
        }

        [TestMethod()]
        public void SatisfiesTest_AllElements_Throws()
        {
            Func<int, int, bool> comparisonFunc = (int number, int biggerThan) => number > biggerThan;

            var compareValue = 10;

            var array = new[] { 9, 15, 18, 11 };
            Assert.ThrowsException<ArgumentException>(() => Array.ForEach(array, (int num) => num.Satisfies(nintern => comparisonFunc(nintern, compareValue))));
        }
    }
}