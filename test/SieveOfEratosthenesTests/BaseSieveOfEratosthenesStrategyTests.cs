using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace SieveOfEratosthenesTests
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using SieveOfEratosthenes;

    [TestClass]
    public class BaseSieveOfEratosthenesStrategyTests
    {
        private readonly BaseSieveOfEratosthenesStrategy target;

        public BaseSieveOfEratosthenesStrategyTests()
        {
            this.target = new BaseSieveOfEratosthenesStrategy();
        }

        [TestMethod]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void IsPrime_LargerOddNumberThanResultSize_ThrowsIndexOutOfRange()
        {
            // Arrange
            var result = this.target.ComputePrimeNumbers(5);

            // Act
            // Assert
            result.IsPrime(7);
        }

        [TestMethod]
        public void IsPrime_LargerEvenNumberThanResultSize_ReturnsFalse()
        {
            // Arrange
            var result = this.target.ComputePrimeNumbers(5);

            // Act
            // Assert
            Assert.IsFalse(result.IsPrime(6));
        }

        [TestMethod]
        public void IsPrime_Number2_ReturnsTrue()
        {
            // Arrange
            var result = this.target.ComputePrimeNumbers(5);

            // Act
            // Assert
            Assert.IsTrue(result.IsPrime(2));
        }

        [TestMethod]
        public void IsPrime_Number5_ReturnsTrue()
        {
            // Arrange
            var result = this.target.ComputePrimeNumbers(5);

            // Act
            // Assert
            Assert.IsTrue(result.IsPrime(5));
        }

        [TestMethod]
        public void IsPrime_Number4_ReturnsFalse()
        {
            // Arrange
            var result = this.target.ComputePrimeNumbers(4);

            // Act
            // Assert
            Assert.IsFalse(result.IsPrime(4));
        }
        
        [TestMethod]
        public void CountPrimes_ComputePrimesTo5_Returns3()
        {
            // Arrange
            var result = this.target.ComputePrimeNumbers(5);

            // Act
            // Assert
            Assert.AreEqual(3, result.CountPrimes());
        }

        [TestMethod]
        public void CountPrimes_ComputePrimesTo2_Return1()
        {
            // Arrange
            var result = this.target.ComputePrimeNumbers(2);

            // Act
            // Assert
            Assert.AreEqual(1, result.CountPrimes());
        }

        [TestMethod]
        public void CountPrimes_ComputePrimesTo1_Returns0()
        {
            // Arrange
            var result = this.target.ComputePrimeNumbers(1);

            // Act
            // Assert
            Assert.AreEqual(0, result. CountPrimes());
        }

        [TestMethod]
        public void CountPrimes_ComputePrimesTo13_Returns6()
        {
            // Arrange
            var result = this.target.ComputePrimeNumbers(13);

            // Act
            // Assert
            Assert.AreEqual(6, result.CountPrimes());
        }

        [TestMethod]
        public void CountPrimes_ComputePrimesTo12_Returns5()
        {
            // Arrange
            var result = this.target.ComputePrimeNumbers(12);

            // Act
            // Assert
            Assert.AreEqual(5, result.CountPrimes());
        }
    }
} 