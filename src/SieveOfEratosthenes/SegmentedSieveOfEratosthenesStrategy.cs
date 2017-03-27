namespace SieveOfEratosthenes
{
    using System;
    using System.Linq;

    public class SegmentedSieveOfEratosthenesStrategy : ISieveOfEratosthenesStrategy
    {
        private const int SegmentSize = 64000;

        public IPrimeNumbersResult ComputePrimeNumbers(long n)
        {
            var myPrimes = new long[6541];
            var nextPrimes = new long[6541];
            var mysize = 0;
            var d = false;
            var sqN = (long)Math.Sqrt(n);

            var result = new bool[(long)(n / 2.0 + 0.5)];

            for (long low = 3; low <= n; low += SegmentSize)
            {
                // current segment = interval [low, high]
                long high;
                if (low + SegmentSize - 1 < n)
                    high = low + SegmentSize - 1;
                else
                    high = n;

                long i;
                long j;
                for (i = 0; i < mysize; i++)
                {
                    j = nextPrimes[i] / 2;
                    long k;
                    for (k = myPrimes[i]; (j * 2) + 1 <= high; j += k)
                    {
                        result[j] = true;
                    }
                    nextPrimes[i] = j * 2 + 1;
                }

                if (!d)
                {
                    for (i = low; i <= high; i = i + 2)
                    {
                        if (i > sqN)
                        {
                            d = true;
                            break;
                        }

                        if (!result[i/2])
                        {
                            mysize++;
                            myPrimes[mysize - 1] = i;
                            for (j = i * i; j <= high; j = j + 2 * i)
                            {
                                result[j/2] = true;
                            }
                            nextPrimes[mysize - 1] = j;
                        }
                    }
                }
            }

            return new PrimeResult(result, n);
        }
        
    }
}