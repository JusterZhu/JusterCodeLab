using System;
using System.Threading;
using DelayMessageApp.Interfaces;

namespace DelayMessageApp.Achieves.Base
{
    public class BaseTask<T> : ITask
    {
        private long _cycle;
        private long _id;
        private T _data;

        public Action<T> TaskAction { get; set; }

        public long Cycle
        {
            get { return Interlocked.Read(ref _cycle); }
            set { Interlocked.Exchange(ref _cycle, value); }
        }

        public long Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public T Data
        {
            get { return _data; }
            set { _data = value; }
        }

        public BaseTask(long cycle, Action<T> action, T data,long id)
        {
            Cycle = cycle;
            TaskAction = action;
            Data = data;
            Id = id;
        }

        public BaseTask(long cycle, Action<T> action,T data)
        {
            Cycle = cycle;
            TaskAction = action;
            Data = data;
        }

        public BaseTask(long cycle, Action<T> action)
        {
            Cycle = cycle;
            TaskAction = action;
        }
        
        public void SubCycleNumber()
        {
            Interlocked.Decrement(ref _cycle);
        }
    }
}
