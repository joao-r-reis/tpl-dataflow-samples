using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleProcessorPipelines
{
    public class CountOddNumbersCommand
    {
        public int? Result { get; private set; }

        public int ListLength { get; private set; }

        public List<int> List { get; private set; }

        public CountOddNumbersCommand(int numbers)
        {
            this.ListLength = numbers;
        }

        public void SetResult(int result)
        {
            this.Result = result;
        }

        public void SetList(List<int> list)
        {
            this.List = list;
        }
    }
}
