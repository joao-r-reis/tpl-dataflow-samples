namespace ConsoleApp
{
    using System;
    using System.Threading.Tasks;
    using System.Threading.Tasks.Dataflow;

    //// <summary>
    //// Example taken from https://msdn.microsoft.com/en-us/library/hh228601(v=vs.110).aspx
    //// The example in the linked page shows a way to implement this pattern with multiple consumers
    //// instead of just one, using the TryReceive method.
    //// </summary>

    // Demonstrates a basic producer and consumer pattern that uses dataflow.
    public static class DataflowProducerConsumer
    {
        public static async Task RunAsync()
        {
            // Create a BufferBlock<byte[]> object. This object serves as the 
            // target block for the producer and the source block for the consumer.
            var buffer = new BufferBlock<byte[]>();

            // Start the consumer. The Consume method runs asynchronously. 
            var consumer = ConsumeAsync(buffer);

            // Post source data to the dataflow block.
            await ProduceAsync(buffer).ConfigureAwait(false);

            // Wait for the consumer to process all data.
            await consumer.ConfigureAwait(false);

            // Print the count of bytes processed to the console.
            Console.WriteLine("Processed {0} bytes.", consumer.Result);
        }

        // Demonstrates the production end of the producer and consumer pattern.
        private static async Task ProduceAsync(ITargetBlock<byte[]> target)
        {
            // Create a Random object to generate random data.
            var rand = new Random();

            // In a loop, fill a buffer with random data and
            // post the buffer to the target block.
            for (var i = 0; i < 100; i++)
            {
                // Create an array to hold random byte data.
                var buffer = new byte[1024];

                // Fill the buffer with random bytes.
                rand.NextBytes(buffer);

                // Post the result to the message block.
                await target.SendAsync(buffer).ConfigureAwait(false);
            }

            // Set the target to the completed state to signal to the consumer
            // that no more data will be available.
            target.Complete();
        }

        // Demonstrates the consumption end of the producer and consumer pattern.
        private static async Task<int> ConsumeAsync(ISourceBlock<byte[]> source)
        {
            // Initialize a counter to track the number of bytes that are processed.
            var bytesProcessed = 0;

            // Read from the source buffer until the source buffer has no 
            // available output data.
            while (await source.OutputAvailableAsync())
            {
                var data = await source.ReceiveAsync().ConfigureAwait(false);

                // Increment the count of bytes received.
                bytesProcessed += data.Length;
            }

            return bytesProcessed;
        }
    }

    /* Output:
    Processed 102400 bytes.
    */
}