namespace SieveOfEratosthenes
{
    using System.Linq;
    
    public class PrimeResult : IPrimeNumbersResult
    {
        private readonly long N;

        private readonly bool[] result;

        private long nrPrimesCount = -1;

        public PrimeResult(bool[] result, long N)
        {
            this.result = result;
            this.N = N;
        }

        public long CountPrimes()
        {
            if (this.nrPrimesCount >= 0)
            {
                return this.nrPrimesCount;
            }

            if (this.N < 2)
            {
                this.nrPrimesCount = 0;
                return this.nrPrimesCount;
            }

            if (this.N == 2)
            {
                this.nrPrimesCount = 1;
                return this.nrPrimesCount;
            }

            this.nrPrimesCount = this.result.Count(p => !p);
            return this.nrPrimesCount;
        }

        public bool IsPrime(long nr)
        {
            if (nr == 2)
            {
                return true;
            }

            if (nr % 2 == 0)
            {
                return false;
            }

            return !this.result[nr / 2];
        }
    }
}