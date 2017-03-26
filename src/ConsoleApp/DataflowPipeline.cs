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
        private readonly ITargetBlock<Tuple<int,int>> entryBlock;
        private readonly IDataflowBlock finalBlock;
        private readonly ConcurrentBag<string> result = new ConcurrentBag<string>();

        private DataflowPipeline()
        {
            ////
            //// Create the members of the pipeline.
            ////

            // Generates string
            var inputBlock = new TransformBlock<Tuple<int, int>, string>(
                t => string.Join(" ", GenerateStrings(t.Item1, t.Item2)),
                new ExecutionDataflowBlockOptions
                {
                    MaxDegreeOfParallelism = 8
                });

            // Separates the specified text into an array of words.
            var splitBlock = new TransformBlock<string, string[]>(
                text => text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries),
                new ExecutionDataflowBlockOptions
                {
                    MaxDegreeOfParallelism = 8
                });

            // Removes duplicates.
            var filterBlock = new TransformBlock<string[], IEnumerable<string>>(words => words.Distinct(), new ExecutionDataflowBlockOptions
            {
                MaxDegreeOfParallelism = 8,
                SingleProducerConstrained = true
            });
            
            // Order by.
            var orderBlock = new TransformManyBlock<IEnumerable<string>, string>(words => words.OrderBy(w => w), new ExecutionDataflowBlockOptions
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

            inputBlock.LinkTo(splitBlock, new DataflowLinkOptions { PropagateCompletion = true });
            splitBlock.LinkTo(filterBlock, new DataflowLinkOptions { PropagateCompletion = true });
            filterBlock.LinkTo(orderBlock, new DataflowLinkOptions { PropagateCompletion = true });
            orderBlock.LinkTo(reverseBlock, new DataflowLinkOptions { PropagateCompletion = true });

            this.entryBlock = inputBlock;
            this.finalBlock = reverseBlock;
        }

        public static async Task RunAsync(int inputSize)
        {
            var pipeline = new DataflowPipeline();

            Console.WriteLine("Sending input messages to pipeline...");
            var sw = new Stopwatch();
            sw.Start();

            // var generatedStrings = new string[inputSize];
            var inputCmd = new Tuple<int, int>(1024, 512);
            for (var i = 0; i < inputSize; i++)
            {
                pipeline.entryBlock.Post(inputCmd);
            }

            //var inputTasks = generatedStrings.Select(s => pipeline.entryBlock.SendAsync(s));
            //generatedStrings = null;
            //await Task.WhenAll(inputTasks).ConfigureAwait(false);
            pipeline.entryBlock.Complete();
            await pipeline.finalBlock.Completion.ConfigureAwait(false);
            sw.Stop();

            Console.WriteLine("Done. Time elapsed: {0} ms", sw.Elapsed.TotalMilliseconds);
            Console.WriteLine("Pipeline complete. Number of results: {0}", pipeline.result.Count);
        }

        public static void RunWithoutPipeline(int inputSize)
        {
            var list = new List<string>();

            Console.WriteLine("Processing without pipeline...");
            var sw = new Stopwatch();
            sw.Start();
            
            for (var i = 0; i < inputSize; i++)
            {
                var generatedString = string.Join(" ", GenerateStrings(1024, 512));
                var strings = generatedString.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                var stringList = strings.Distinct().OrderBy(ss => ss);
                list.AddRange(stringList.Select(str => new string(str.Reverse().ToArray())));
            }
            
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