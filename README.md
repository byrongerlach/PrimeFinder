# PrimeFinder
A tool to find the largest prime number in 60 seconds. Developed using C# and MVVM Light.

## Maximum Found Prime in 60 Seconds

Using the tool, the maximum prime I found was: 93,564,257. This is running the tool from the Release configuration executable. 

## Running The Project

Download the code from this repository, build the Release configuration using Visual Studio, then run it from the executable:  

``PrimeFinder\PrimeFinder\bin\Release\PrimeFinder.exe``

## Design

I used WPF and the MVVM Light template for a simple GUI to display the timer, prime number updates, and the status message showing when the tool was finished running.

## Algorithm

I chose the Sieve of Erastosthenes after searching for algorithmic approaches: https://en.wikipedia.org/wiki/Sieve_of_Eratosthenes . The algorithm was relatively simple and fast. The downside I soon experienced was that it consumes a lot of memory due to the fact that it stores lookup tables for non-prime numbers. To deal with this, I imposed a limit of 100,000,000 for the maximum size of a prime that the program can find. Additionally, I stored a limited number of found  primes and did a check against prime candidates to see if they were divisible by the list of known primes. Both of these had the effect of reducing the size of the non-prime lookup structure. Given more time, I could try to segment the sieve, or I could try another algorithm.
