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
            Console.WriteLine("Base Algorithm...");

            var calculator = new SieveOfErasthotenesCalculator();
            
            calculator.ComputePrimeNumbers(SieveOfErasthotenesCalculator.AlgorithmVersion.Base, (long)Math.Pow(2, 31));

            Console.WriteLine("Parallel Algorithm...");

            calculator = new SieveOfErasthotenesCalculator();

            calculator.ComputePrimeNumbers(SieveOfErasthotenesCalculator.AlgorithmVersion.Parallel, (long)Math.Pow(2, 31));

            Console.WriteLine("Segmented Algorithm...");

            calculator = new SieveOfErasthotenesCalculator();

            calculator.ComputePrimeNumbers(SieveOfErasthotenesCalculator.AlgorithmVersion.Segmented, (long)Math.Pow(2, 31));

            calculator = null;

            Console.WriteLine("Press enter to exit...");
            Console.ReadLine();
            return Task.CompletedTask;
        }
    }
}