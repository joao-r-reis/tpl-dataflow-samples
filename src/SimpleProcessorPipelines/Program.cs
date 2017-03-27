using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleProcessorPipelines
{
    public class Program
    {
        public static void Main(string[] args)
        {
            MainAsync(args).Wait();
        }

        private static async Task MainAsync(string[] args)
        {
            // warming up
            Console.WriteLine("Warming up...");
            await new OddNumberCounter().CountOddNumbers(2, (int) Math.Pow(2, 15)).ConfigureAwait(false);

            Console.WriteLine("Done. Starting pipeline...");
            
            var result = await new OddNumberCounter().CountOddNumbers(24, (int)Math.Pow(2, 20)).ConfigureAwait(false);

            Console.WriteLine("Counted {0} odd numbers.", result.Sum(cmd => cmd.Result));

            Console.WriteLine("Press enter to exit...");
            Console.ReadLine();
        }
    }
}
