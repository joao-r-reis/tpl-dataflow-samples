using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JoinBlockExample
{
    public class ServiceProducts
    {
        public async Task<Product> GetProductAsync(int id)
        {
            var rand = new Random();
            await Task.Delay(rand.Next(90, 110)).ConfigureAwait(false);
            return new Product
            {
                Id = id,
                Name = rand.Next().ToString()
            };
        }
    }
}
