namespace ConsoleApp
{
    using System;
    using System.Collections;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Threading.Tasks.Dataflow;

    public class DataflowPipeline
    {
        private readonly ITargetBlock<string> entryBlock;
        private readonly IDataflowBlock finalBlock;
        private readonly ConcurrentBag<string> result = new ConcurrentBag<string>();

        private DataflowPipeline()
        {
            ////
            //// Create the members of the pipeline.
            ////

            // Separates the specified text into an array of words.
            var splitBlock = new TransformBlock<string, string[]>(
                text => text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries),
                new ExecutionDataflowBlockOptions
                {
                    MaxDegreeOfParallelism = 8
                });

            // Removes duplicates.
            var filterBlock = new TransformManyBlock<string[], string>(words => words.Distinct(), new ExecutionDataflowBlockOptions
            {
                MaxDegreeOfParallelism = 8
            });

            // Reverse words and add to result list
            var reverseBlock = new ActionBlock<string>(
                word =>
                {
                    var str = new string(word.Reverse().ToArray());
                    this.result.Add(str);
                }, new ExecutionDataflowBlockOptions
                {
                    MaxDegreeOfParallelism = 8
                });

            ////
            //// Link members of the pipeline together (and propagate completion)
            ////

            splitBlock.LinkTo(filterBlock, new DataflowLinkOptions { PropagateCompletion = true });
            filterBlock.LinkTo(reverseBlock, new DataflowLinkOptions { PropagateCompletion = true });

            this.entryBlock = splitBlock;
            this.finalBlock = reverseBlock;
        }

        public static async Task RunAsync(int inputSize)
        {
            var pipeline = new DataflowPipeline();

            var generatedStrings = new string[inputSize];
            for (var i = 0; i < generatedStrings.Length; i++)
            {
                generatedStrings[i] = string.Join(" ", GenerateStrings(1024, 512));
            }

            Console.WriteLine("Sending input messages to pipeline...");
            var sw = new Stopwatch();
            sw.Start();
            var inputTasks = generatedStrings.Select(s => pipeline.entryBlock.SendAsync(s));
            generatedStrings = null;
            await Task.WhenAll(inputTasks).ConfigureAwait(false);
            pipeline.entryBlock.Complete();
            await pipeline.finalBlock.Completion.ConfigureAwait(false);
            sw.Stop();

            Console.WriteLine("Done. Time elapsed: {0} ms", sw.Elapsed.TotalMilliseconds);
            Console.WriteLine("Pipeline complete. Number of results: {0}", pipeline.result.Count);
        }

        public static void RunWithoutPipeline(int inputSize)
        {
            var list = new List<string>();

            var generatedStrings = new string[inputSize];
            for (var i = 0; i < generatedStrings.Length; i++)
            {
                generatedStrings[i] = string.Join(" ", GenerateStrings(1024, 512));
            }

            Console.WriteLine("Processing without pipeline...");
            var sw = new Stopwatch();
            sw.Start();

            foreach (var s in generatedStrings)
            {
                var strings = s.Split(new char[] {' '}, StringSplitOptions.RemoveEmptyEntries);
                strings = strings.Distinct().ToArray();
                list.AddRange(strings.Select(str => new string(str.Reverse().ToArray())));
            }

            generatedStrings = null;
            sw.Stop();

            Console.WriteLine("Done. Time elapsed: {0} ms", sw.Elapsed.TotalMilliseconds);

            Console.WriteLine("Processing complete. Number of results: {0}", list.Count);
        }

        private static IEnumerable<string> GenerateStrings(int nr, int strLen)
        {
            const string Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            var stringChars = new char[strLen];
            var generatedStrings = new string[nr];

            for (var j = 0; j < generatedStrings.Length; j++)
            {
                for (var i = 0; i < stringChars.Length; i++)
                {
                    stringChars[i] = Chars[random.Next(Chars.Length)];
                }

                var finalString = new string(stringChars);
                generatedStrings[j] = finalString;
            }

            return generatedStrings;
        }
    }
}