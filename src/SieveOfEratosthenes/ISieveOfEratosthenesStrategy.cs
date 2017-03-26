namespace SieveOfEratosthenes
{
    internal interface ISieveOfEratosthenesStrategy
    {
        IPrimeNumbersResult ComputePrimeNumbers(long n);
    }
}