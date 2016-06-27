using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PrimeFinder.Model
{
    /// <summary>
    /// Finds the greatest prime number
    /// </summary>
    public class PrimeFinderModel : IDisposable
    {
        public int MaxPrime { get; set; }

        public int EndPrimeRange { get; set; }

        public int StartPrimeRange { get; set; }

        // Numbers found that are primes
        private HashSet<int> Primes;

        // Numbers found that are not primes
        private HashSet<int> NotPrimes = new HashSet<int>();

        private CancellationTokenSource Cts { get; set; }

        // The event handler for when the count threshold is reached.
        public event EventHandler PrimeFound;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="endPrimeRange"></param>
        public PrimeFinderModel(int endPrimeRange)
        {
            EndPrimeRange = endPrimeRange;
        }

        /// <summary>
        /// Checks to see if a number is prime based on the known list.
        /// </summary>
        /// <param name="number">The number to check</param>
        public bool IsPrime(int number)
        {
            return true;
        }
        
        /// <summary>
        /// Start the prime searching task
        /// </summary>
        public void Start()
        {
            Cts = new CancellationTokenSource();
            Task.Factory.StartNew(() => FindMaxPrime(Cts), Cts.Token);
        }

        // <summary>
        // Starting from 2, keep finding the next larger prime number until cancelled.
        // This method uses the "Sieve of Eratosthenes" as explained here: https://en.wikipedia.org/wiki/Sieve_of_Eratosthenes
        // 1. Create a list of consecutive integers from 2 through n: (2, 3, 4, ..., n).
        // 2. Initially, let p equal 2, the smallest prime number.
        // 3. Enumerate the multiples of p by counting to n from 2p in increments of p, and mark them in the list (these will be 2p, 3p, 4p, ... ; the p itself should not be marked).
        // 4. Find the first number greater than p in the list that is not marked. If there was no such number, stop. 
        // Otherwise, let p now equal this new number(which is the next prime), and repeat from step 3.        
        /// </summary>
        public void FindMaxPrime(CancellationTokenSource cts)
        {            
            MaxPrime = 2;

            int currentNumber = 0;
            int outerCounter = 0;

            var outerTotal = EndPrimeRange - MaxPrime;

            while (outerCounter++ <= outerTotal)
            {                
                for (currentNumber = MaxPrime; currentNumber <= EndPrimeRange; currentNumber++)
                {
                    if (cts.IsCancellationRequested)
                        return;

                    NotPrimes.Add(currentNumber * MaxPrime);
                }

                for (currentNumber = MaxPrime + 1; currentNumber <= EndPrimeRange; currentNumber++)
                {
                    if (cts.IsCancellationRequested)
                        return;

                    if (!NotPrimes.Contains(currentNumber))
                    {
                        MaxPrime = currentNumber;
                        PrimeFound?.Invoke(this, EventArgs.Empty);
                        break;
                    }
                }
            }
            
        }
        
        /// <summary>
        /// Cancel the FindPrimes task
        /// </summary>
        public void StopPrimeFinder()
        {
            Cts.Cancel();
        }

        /// <summary>
        /// Dispose of the cancellation token source
        /// </summary>
        public void Dispose()
        {
            Cts.Dispose();
        }
    }

}
