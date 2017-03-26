namespace SieveOfEratosthenes
{
    using System;
    using System.Threading.Tasks;

    public class Program
    {
        public static void Main(string[] args)
        {
            MainAsync(args).Wait();
        }

        private static Task MainAsync(string[] args)
        {
            var calculator = new SieveOfErasthotenesCalculator();
            
            var result = calculator.ComputePrimeNumbers(SieveOfErasthotenesCalculator.AlgorithmVersion.Base, (long)Math.Pow(2, 30));

            Console.WriteLine("Press enter to exit...");
            Console.ReadLine();
            return Task.CompletedTask;
        }
    }
}