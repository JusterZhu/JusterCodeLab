using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DelayMessageApp.Achieves.Base;

namespace DelayMessageApp.Achieves.Minute
{
    public class MinuteTask<T> : BaseTask<T>
    {
        public MinuteTask(long cycle, Action<T> action, T data) : base(cycle, action, data)
        {
        }

        public MinuteTask(long cycle, Action<T> action) : base(cycle, action)
        {
        }
    }
}
