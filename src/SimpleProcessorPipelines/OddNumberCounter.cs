using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace SimpleProcessorPipelines
{
    public class OddNumberCounter
    {
        private readonly ITargetBlock<CountOddNumbersCommand> firstBlock;
        private readonly IDataflowBlock finalBlock;

        public OddNumberCounter()
        {
            var firstBlock = new TransformBlock<CountOddNumbersCommand, CountOddNumbersCommand>(async cmd =>
            {
                var list = new List<int>();
                var rand = new Random();

                var pausedTime = rand.Next(500);
                Console.WriteLine("Waiting {0} ms.", pausedTime);
                await Task.Delay(pausedTime).ConfigureAwait(false);

                for (var i = 0; i < cmd.ListLength; i++)
                {
                    list.Add(rand.Next());
                }

                Console.WriteLine("Generated {0} numbers.", cmd.ListLength);
                cmd.SetList(list);
                return cmd;
            },
            new ExecutionDataflowBlockOptions
            {
                EnsureOrdered = true,
                MaxDegreeOfParallelism = 20,
                SingleProducerConstrained = true,
                BoundedCapacity = 1000
            });

            var finalBlock = new ActionBlock<CountOddNumbersCommand>(async cmd =>
                {
                    var rand = new Random();
                    var pausedTime = rand.Next(500);
                    Console.WriteLine("Waiting {0} ms.", pausedTime);
                    await Task.Delay(pausedTime).ConfigureAwait(false);
                    var count = cmd.List.Count(t => t%2 == 1);
                    Console.WriteLine("Counted {0} odd numbers in current batch.", count);
                    cmd.SetResult(count);
                },
                new ExecutionDataflowBlockOptions
                {
                    EnsureOrdered = true,
                    MaxDegreeOfParallelism = 20,
                    SingleProducerConstrained = false,
                    BoundedCapacity = 1000
                });

            firstBlock.LinkTo(finalBlock, new DataflowLinkOptions {PropagateCompletion = true});

            this.finalBlock = finalBlock;
            this.firstBlock = firstBlock;
        }

        public async Task<List<CountOddNumbersCommand>> CountOddNumbers(int batches, int batchSize)
        {
            var result = new List<CountOddNumbersCommand>();

            for (var i = 0; i < batches; i++)
            {
                var cmd = new CountOddNumbersCommand(batchSize);
                result.Add(cmd);
                Console.WriteLine("Sending batch {0} with batchSize {1}.", i, batchSize);
                await this.firstBlock.SendAsync(cmd).ConfigureAwait(false);
            }
            this.firstBlock.Complete();

            Console.WriteLine("Waiting for completion.");
            await this.finalBlock.Completion.ConfigureAwait(false);
            Console.WriteLine("Completed!");

            return result;
        }
    }
}
