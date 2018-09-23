using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DelayMessageApp.Interfaces;

namespace DelayMessageApp.Achieves.Base
{
    public abstract class BaseQueue<T> : IRingQueue<T>
    {
        private long _pointer = 0L;
        private ConcurrentBag<BaseTask<T>>[] _arraySlot;
        private int ArrayMax;

        /// <summary>
        /// Ring queue.
        /// </summary>
        public ConcurrentBag<BaseTask<T>>[] ArraySlot
        {
            get { return _arraySlot ?? (_arraySlot = new ConcurrentBag<BaseTask<T>>[ArrayMax]); }
        }
        
        public BaseQueue(int arrayMax)
        {
            if (arrayMax < 60 && arrayMax % 60 == 0)
                throw new Exception("Ring queue length cannot be less than 60 and is a multiple of 60 .");

            ArrayMax = arrayMax;
        }

        public void Add(long delayTime, Action<T> action)
        {
            Add(delayTime, action, default(T));
        }

        public void Add(long delayTime,Action<T> action,T data)
        {
            Add(delayTime, action, data,0);
        }

        public void Add(long delayTime, Action<T> action, T data,long id)
        {
            NextSlot(delayTime, out long cycle, out long pointer);
            ArraySlot[pointer] =  ArraySlot[pointer] ?? (ArraySlot[pointer] = new ConcurrentBag<BaseTask<T>>());
            var baseTask = new BaseTask<T>(cycle, action, data,id);
            ArraySlot[pointer].Add(baseTask);
        }

        /// <summary>
        /// Remove tasks based on ID.
        /// </summary>
        /// <param name="id"></param>
        public void Remove(long id)
        {
            try
            {
                Parallel.ForEach(ArraySlot, (ConcurrentBag<BaseTask<T>> collection, ParallelLoopState state) =>
                {
                    var resulTask = collection.FirstOrDefault(p => p.Id == id);
                    if (resulTask != null)
                    {
                        collection.TryTake(out resulTask);
                        state.Break();
                    }
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        
        public void Start()
        {
            while (true)
            {
                RightMovePointer();
                Thread.Sleep(1000);
                Console.WriteLine(DateTime.Now.ToString());
            }
        }

        /// <summary>
        /// Calculate the information of the next slot.
        /// </summary>
        /// <param name="delayTime">Delayed execution time.</param>
        /// <param name="cycle">Number of turns.</param>
        /// <param name="index">Task location.</param>
        private void NextSlot(long delayTime, out long cycle,out long index)
        {
            try
            {
                var circle = delayTime / ArrayMax;
                var second = delayTime % ArrayMax;
                var current_pointer = GetPointer();
                var queue_index = 0L;

                if (delayTime - ArrayMax > ArrayMax)
                {
                    circle = 1;
                }
                else if (second > ArrayMax)
                {
                    circle += 1;
                }

                if (delayTime - circle * ArrayMax < ArrayMax)
                {
                    second = delayTime - circle * ArrayMax;
                }

                if (current_pointer + delayTime >= ArrayMax)
                {
                    cycle = (int)((current_pointer + delayTime) / ArrayMax);
                    if (current_pointer + second - ArrayMax < 0)
                    {
                        queue_index = current_pointer + second;
                    }
                    else if (current_pointer + second - ArrayMax > 0)
                    {
                        queue_index = current_pointer + second - ArrayMax;
                    }
                }
                else
                {
                    cycle = 0;
                    queue_index = current_pointer + second;
                }
                index = queue_index;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        /// <summary>
        /// Get the current location of the pointer.
        /// </summary>
        /// <returns></returns>
        private long GetPointer()
        {
            return Interlocked.Read(ref _pointer);
        }

        /// <summary>
        /// Reset pointer position.
        /// </summary>
        private void ReSetPointer()
        {
            Interlocked.Exchange(ref _pointer, 0);
        }

        /// <summary>
        /// Pointer moves clockwise.
        /// </summary>
        private void RightMovePointer()
        {
            try
            {
                if (GetPointer() >= ArrayMax - 1)
                {
                    ReSetPointer();
                }
                else
                {
                    Interlocked.Increment(ref _pointer);
                }

                var pointer = GetPointer();
                var taskCollection = ArraySlot[pointer];
                if (taskCollection == null || taskCollection.Count == 0) return;

                Parallel.ForEach(taskCollection, (BaseTask<T> task) =>
                {
                    if (task.Cycle > 0)
                    {
                        task.SubCycleNumber();
                    }

                    if (task.Cycle <= 0)
                    {
                        taskCollection.TryTake(out task);
                        task.TaskAction(task.Data);
                    }
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
