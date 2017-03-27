using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JoinBlockExample
{
    public class ServicePrices
    {
        public async Task<double> GetPriceByProductId(int productId)
        {
            var rand = new Random();

            await Task.Delay(rand.Next(90, 110)).ConfigureAwait(false);
            
            return rand.NextDouble() * rand.Next();
        }
    }
}
