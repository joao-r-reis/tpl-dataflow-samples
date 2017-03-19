namespace ConsoleApp
{
    using System;
    using System.Diagnostics;
    using System.Threading.Tasks;

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
            //await DataflowPipeline.RunAsync(400).ConfigureAwait(false);
            DataflowPipeline.RunWithoutPipeline(200);
            Console.WriteLine("Done. Starting pipeline...");
            
            // await DataflowProducerConsumer.RunAsync().ConfigureAwait(false);
            //await DataflowPipeline.RunAsync(400).ConfigureAwait(false);
            DataflowPipeline.RunWithoutPipeline(400);

            Console.WriteLine("Press enter to exit...");
            Console.ReadLine();
        }
    }
}