namespace SieveOfEratosthenes
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    public class SieveOfErasthotenesCalculator
    {
        public enum AlgorithmVersion
        {
            Base,
            Parallel,
            Segmented
        }

        private readonly IDictionary<AlgorithmVersion, ISieveOfEratosthenesStrategy> strategies;

        public SieveOfErasthotenesCalculator()
        {
            this.strategies = new Dictionary<AlgorithmVersion, ISieveOfEratosthenesStrategy>
            {
                { AlgorithmVersion.Base, new BaseSieveOfEratosthenesStrategy() },
                { AlgorithmVersion.Parallel, new ParallelSieveOfEratosthenesStrategy() },
                { AlgorithmVersion.Segmented, new SegmentedSieveOfEratosthenesStrategy() }
            };
        }

        public IPrimeNumbersResult ComputePrimeNumbers(AlgorithmVersion version, long maxNumber)
        {
            var strategy = this.strategies[version];

            var sw = new Stopwatch();

            sw.Start();
            var result = strategy.ComputePrimeNumbers(maxNumber);
            sw.Stop();

            Console.WriteLine("Time elapsed: {0} ms.", sw.Elapsed.TotalMilliseconds);
            Console.WriteLine("Total prime numbers: {0}, N = {1}", result.CountPrimes(), maxNumber);

            return result;
        }
    }
}