using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DelayMessageApp.Achieves.Base;

namespace DelayMessageApp.Achieves.Minute
{
    public class MinuteQueue<T> : BaseQueue<T>
    {
        private const int ArrayMax = 60;

        public MinuteQueue() : base(ArrayMax)
        {
            
        }
    }
}
