using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JoinBlockExample
{
    public class ProductQueryResult
    {
        public List<Store> Stores { get; set; }

        public double Price { get; set; }

        public Product Product { get; set; }
    }
}
