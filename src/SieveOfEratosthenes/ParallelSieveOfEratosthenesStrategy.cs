namespace SieveOfEratosthenes
{
    using System;
    using System.Threading.Tasks;
    using System.Linq;

    public class ParallelSieveOfEratosthenesStrategy : ISieveOfEratosthenesStrategy
    {
        public IPrimeNumbersResult ComputePrimeNumbers(long n)
        {
            var result = new bool[(long) (n / 2.0D + 0.5D)];
            var maxI = (long) Math.Sqrt(n);

            for (int i = 3; i <= maxI; i = i + 2)
            {
                if (!result[i / 2])
                {
                    var j = (i*i)/2;

                    var maxIndexDouble = (double)(result.Length - j) / i;
                    var maxIndexInt = (long)maxIndexDouble;

                    if (maxIndexDouble > maxIndexInt)
                    {
                        maxIndexInt++;
                    }
                    
                    Parallel.For(0, maxIndexInt, index =>
                    {
                        var cenas = j + i * index;
                        result[cenas] = true;
                    });
                }
            }

            return new PrimeResult(result, n);
        }
    }
}