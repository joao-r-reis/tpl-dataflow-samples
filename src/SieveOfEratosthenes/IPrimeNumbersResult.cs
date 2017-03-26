namespace SieveOfEratosthenes
{
    public interface IPrimeNumbersResult
    {
        long CountPrimes();

        bool IsPrime(long nr);
    }
}