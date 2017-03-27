using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace JoinBlockExample
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
            await new ProductsPipeline().GetProducts((int) Math.Pow(2, 10)).ConfigureAwait(false);

            Console.WriteLine("Done. Starting pipeline...");
            
            var sw = new Stopwatch();
            sw.Start();
            var result = await new ProductsPipeline().GetProducts((int)Math.Pow(2, 22)).ConfigureAwait(false);
            sw.Stop();

            Console.WriteLine("Queried {0} products in {1} ms", result.Count, sw.Elapsed.TotalMilliseconds);

            Console.WriteLine("Press enter to exit...");
            Console.ReadLine();
        }
    }
}
