using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DelayMessageApp.Achieves.Minute;
using DelayMessageApp.Interfaces;
using DelayMessageApp.Models;

namespace DelayMessageApp.Logic
{
    public class UseLogic
    {
        public static void Start()
        {
            //1.Initialize queues of different granularity.
            IRingQueue<NewsModel> minuteRingQueue = new MinuteQueue<NewsModel>();

            //2.Open thread.
            var lstTasks = new List<Task>
            {
                Task.Factory.StartNew(minuteRingQueue.Start)
            };

            //3.Add tasks performed in different periods.
            minuteRingQueue.Add(5, new Action<NewsModel>((NewsModel newsObj) =>
            {
                Console.WriteLine(newsObj.News);
            }), new NewsModel() { News = "Trump's visit to China！" });

            minuteRingQueue.Add(10, new Action<NewsModel>((NewsModel newsObj) =>
            {
                Console.WriteLine(newsObj.News);
            }), new NewsModel() { News = "Putin Pu's visit to China！" });

            minuteRingQueue.Add(60, new Action<NewsModel>((NewsModel newsObj) =>
            {
                Console.WriteLine(newsObj.News);
            }), new NewsModel() { News = "Eisenhower's visit to China！" });

            minuteRingQueue.Add(120, new Action<NewsModel>((NewsModel newsObj) =>
            {
                Console.WriteLine(newsObj.News);
            }), new NewsModel() { News = "Xi Jinping's visit to the US！" });

            //3.Waiting for all tasks to complete is usually not completed. Because there is an infinite loop.
            //F5 Run the program and see the effect.
            Task.WaitAll(lstTasks.ToArray());
            Console.Read();
        }
    }
}
