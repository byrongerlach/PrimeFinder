using PrimeFinder.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;

namespace PrimeFinderTest
{
    [TestClass]
    public class PrimeFinderModelTest
    {
        /// <summary>
        /// Check some known prime values.
        /// </summary>
        [TestMethod]
        public void IsPrimeTest()
        {
            var cts = new CancellationTokenSource();

            var primeFinderModel = new PrimeFinderModel(20);
            primeFinderModel.FindMaxPrime(cts);
            Assert.AreEqual(19, primeFinderModel.MaxPrime);

            primeFinderModel = new PrimeFinderModel(100);
            primeFinderModel.FindMaxPrime(cts);
            Assert.AreEqual(97, primeFinderModel.MaxPrime);

            primeFinderModel = new PrimeFinderModel(7000);
            primeFinderModel.FindMaxPrime(cts);
            Assert.AreEqual(6997, primeFinderModel.MaxPrime);

            primeFinderModel = new PrimeFinderModel(19000);
            primeFinderModel.FindMaxPrime(cts);
            Assert.AreEqual(18979, primeFinderModel.MaxPrime);

            primeFinderModel = new PrimeFinderModel(2000000);
            primeFinderModel.FindMaxPrime(cts);
            Assert.AreEqual(1999993, primeFinderModel.MaxPrime);
        }
    }
}
