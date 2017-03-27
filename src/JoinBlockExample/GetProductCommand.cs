using System.Collections.Generic;

namespace JoinBlockExample
{
    public class GetProductCommand
    {
        public ProductCommandResult Result { get; private set; }

        public int ProductId { get; private set; }

        public GetProductCommand(int id)
        {
            this.ProductId = id;
        }

        public void SetResult(ProductCommandResult result)
        {
            this.Result = result;
        }
    }
}
