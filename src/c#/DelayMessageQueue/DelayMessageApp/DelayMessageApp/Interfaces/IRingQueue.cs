using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelayMessageApp.Interfaces
{
    public interface IRingQueue<T>
    {
        /// <summary>
        /// Add tasks [add tasks will automatically generate: task Id, task slot location, number of task cycles]
        /// </summary>
        /// <param name="delayTime">The specified task is executed after N seconds.</param>
        /// <param name="action">Definitions of callback</param>
        void Add(long delayTime,Action<T> action);

        /// <summary>
        /// Add tasks [add tasks will automatically generate: task Id, task slot location, number of task cycles]
        /// </summary>
        /// <param name="delayTime">The specified task is executed after N seconds.</param>
        /// <param name="action">Definitions of callback.</param>
        /// <param name="data">Parameters used in the callback function.</param>
        void Add(long delayTime, Action<T> action, T data);

        /// <summary>
        /// Add tasks [add tasks will automatically generate: task Id, task slot location, number of task cycles]
        /// </summary>
        /// <param name="delayTime"></param>
        /// <param name="action">Definitions of callback</param>
        /// <param name="data">Parameters used in the callback function.</param>
        /// <param name="id">Task ID, used when deleting tasks.</param>
        void Add(long delayTime, Action<T> action, T data, long id);

        /// <summary>
        /// Remove tasks [need to know: where the task is, which specific task].
        /// </summary>
        /// <param name="index">Task slot location</param>
        /// <param name="id">Task ID, used when deleting tasks.</param>
        void Remove(long id);

        /// <summary>
        /// Launch queue.
        /// </summary>
        void Start();
    }
}
