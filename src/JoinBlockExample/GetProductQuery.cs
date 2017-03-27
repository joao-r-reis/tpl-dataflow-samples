using System.Collections.Generic;

namespace JoinBlockExample
{
    public class GetProductQuery
    {
        public ProductQueryResult Result { get; private set; }

        public int ProductId { get; private set; }

        public GetProductQuery(int id)
        {
            this.ProductId = id;
        }

        public void SetResult(ProductQueryResult result)
        {
            this.Result = result;
        }
    }
}
