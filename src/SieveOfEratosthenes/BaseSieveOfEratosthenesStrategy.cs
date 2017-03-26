namespace SieveOfEratosthenes
{
    using System;
    using System.Linq;

    public class BaseSieveOfEratosthenesStrategy : ISieveOfEratosthenesStrategy
    {
        public IPrimeNumbersResult ComputePrimeNumbers(long n)
        {
            var result = new bool[(long) (n / 2.0 + 0.5)];
            var maxI = (long) Math.Sqrt(n);

            for (long i = 3; i <= maxI; i = i + 2)
            {
                if (!result[i/2])
                {
                    for (var j = (i*i)/2; j < result.Length; j = j + i)
                    {
                        result[j] = true;
                    }
                }
            }

            return new PrimeResult(result, n);
        }
    }
}