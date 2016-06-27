using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PrimeFinder.Model
{
    /// <summary>
    /// Finds the greatest prime number
    /// </summary>
    public sealed class PrimeFinderModel : IDisposable
    {
        public int MaxPrime { get; set; }

        public int EndPrimeRange { get; set; }

        public int StartPrimeRange { get; set; }
                
        // Numbers found that are not primes
        private HashSet<int> NotPrimes = new HashSet<int>();

        // Numbers found that are primes
        private List<int> Primes = new List<int>();

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

            // Add the first prime
            Primes.Add(MaxPrime);

            while (outerCounter++ <= outerTotal)                                 
            {        
                for (currentNumber = MaxPrime; currentNumber <= EndPrimeRange; currentNumber++)
                {
                    if (cts.IsCancellationRequested)
                        return;

                    var notPrime = currentNumber * MaxPrime;

                    // Don't add it to the list if it's beyond the maximum range, or less than the already known max prime.
                    if (notPrime > EndPrimeRange)
                        break;

                    if (notPrime < MaxPrime)
                        continue;

                    // Check against some known prime numbers.                    
                    if (IsDivisibleByKnownPrimes(notPrime))
                        continue;

                    // Add it to the known list of not primes
                    NotPrimes.Add(notPrime);
                }

                // Find the prime candidates greater than MaxPrime
                for (currentNumber = MaxPrime + 1; currentNumber <= EndPrimeRange; currentNumber++)
                {
                    if (cts.IsCancellationRequested)
                        return;
                    
                    // Don't add it to the list if it's beyond the maximum range, or less than the already known max prime.
                    if (currentNumber > EndPrimeRange || currentNumber < MaxPrime)
                        continue;

                    if (IsDivisibleByKnownPrimes(currentNumber))
                        continue;
                                        
                    // Check to see if we've found a new maximum prime
                    if (!NotPrimes.Contains(currentNumber))
                    {
                        MaxPrime = currentNumber;

                        // Only keep a limited list of known primes to reduce checking time.
                        if (Primes.Count < 30)
                            Primes.Add(MaxPrime);

                        // Notify the view model that it's time for an update.
                        PrimeFound?.Invoke(this, EventArgs.Empty);
                        break;
                    }
                }                
            }
            
        }
       
        /// <summary>
        /// Check to see if the number is divisible by any primes
        /// </summary>
        /// <param name="number">The number to check</param>
        /// <returns>True, if it is divisible</returns>
                 
        private bool IsDivisibleByKnownPrimes(int number)
        {
            foreach (var prime in Primes)
            {
                if (number % prime == 0)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Cancel the FindPrimes task
        /// </summary>
        public void StopPrimeFinder()
        {
            // Cancel the prime search
            Cts.Cancel();

            // Clean up our tracking hash set.
            NotPrimes.Clear();            
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
