using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;

namespace JoinBlockExample
{
    public class ServiceStores
    {
        public async Task<List<Store>> GetStoresByProductId(int productId)
        {
            var rand = new Random();

            await Task.Delay(rand.Next(90, 110)).ConfigureAwait(false);

            var nrStores = rand.Next(10);
            var result = new List<Store>();

            for (var i = 0; i < nrStores; i++)
            {
                result.Add(new Store
                {
                    Name = rand.Next().ToString(),
                    Id = rand.Next(),
                    ProductId = productId
                });
            }

            return result;
        }
    }
}
