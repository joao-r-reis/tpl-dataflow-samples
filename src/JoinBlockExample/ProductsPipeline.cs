using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace JoinBlockExample
{
    public class ProductsPipeline
    {
        private readonly ITargetBlock<GetProductCommand> firstBlock;
        private readonly IDataflowBlock finalBlock;

        public ProductsPipeline()
        {
            var firstBlock = new BufferBlock<GetProductCommand>(
            new ExecutionDataflowBlockOptions
            {
                MaxDegreeOfParallelism = 20,
                SingleProducerConstrained = true
            });

            var storeBlock = new TransformBlock<GetProductCommand, List<Store>>(
                cmd => new ServiceStores().GetStoresByProductId(cmd.ProductId),
                new ExecutionDataflowBlockOptions
                {
                    MaxDegreeOfParallelism = 20
                });

            var priceBlock = new TransformBlock<GetProductCommand, double>(
                cmd => new ServicePrices().GetPriceByProductId(cmd.ProductId),
                new ExecutionDataflowBlockOptions
                {
                    MaxDegreeOfParallelism = 20
                });

            var productBlock = new TransformBlock<GetProductCommand, Product>(
                cmd => new ServiceProducts().GetProductAsync(cmd.ProductId),
                new ExecutionDataflowBlockOptions
                {
                    MaxDegreeOfParallelism = 20
                });

            var joinBlock = new JoinBlock<Product, List<Store>, double>(new GroupingDataflowBlockOptions
            {
                Greedy = false
            });

            var finalJoinBlock = new JoinBlock<Tuple<Product, List<Store>, double>, GetProductCommand>(new GroupingDataflowBlockOptions
            {
                Greedy = false
            });

            var finalBlock = new ActionBlock<Tuple<Tuple<Product, List<Store>, double>, GetProductCommand>>(tuple =>
                {
                    tuple.Item2.SetResult(new ProductCommandResult
                    {
                        Product = tuple.Item1.Item1,
                        Stores = tuple.Item1.Item2,
                        Price = tuple.Item1.Item3
                    });
                },
            new ExecutionDataflowBlockOptions
            {
                EnsureOrdered = true,
                MaxDegreeOfParallelism = 20,
                SingleProducerConstrained = false
            });

            firstBlock.LinkTo(storeBlock, new DataflowLinkOptions {PropagateCompletion = true});
            firstBlock.LinkTo(productBlock, new DataflowLinkOptions { PropagateCompletion = true });
            firstBlock.LinkTo(priceBlock, new DataflowLinkOptions { PropagateCompletion = true });
            firstBlock.LinkTo(finalJoinBlock.Target2, new DataflowLinkOptions { PropagateCompletion = false });
            productBlock.LinkTo(joinBlock.Target1, new DataflowLinkOptions { PropagateCompletion = true });
            storeBlock.LinkTo(joinBlock.Target2, new DataflowLinkOptions { PropagateCompletion = true });
            priceBlock.LinkTo(joinBlock.Target3, new DataflowLinkOptions { PropagateCompletion = true });
            joinBlock.LinkTo(finalJoinBlock.Target1, new DataflowLinkOptions {PropagateCompletion = true});
            finalJoinBlock.LinkTo(finalBlock, new DataflowLinkOptions {PropagateCompletion = true});

            this.finalBlock = finalBlock;
            this.firstBlock = firstBlock;
        }

        public async Task<List<ProductCommandResult>> GetProducts(int numberOfProducts)
        {
            var rand = new Random();
            var cmds = new List<GetProductCommand>();
            for (var i = 0; i < numberOfProducts; i++)
            {
                var id = rand.Next();
                var cmd = new GetProductCommand(id);
                cmds.Add(cmd);
                //Console.WriteLine("Sending command product id {0}.", i, id);
                await this.firstBlock.SendAsync(cmd).ConfigureAwait(false);
            }
            this.firstBlock.Complete();

            Console.WriteLine("Waiting for completion.");
            await this.finalBlock.Completion.ConfigureAwait(false);
            Console.WriteLine("Completed!");

            return cmds.Select(cmd => cmd.Result).ToList();
        }
    }
}
