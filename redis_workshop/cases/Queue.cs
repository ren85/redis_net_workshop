using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace redis_workshop.cases
{
    //implement queue using redis lists
    //https://redislabs.com/redis-best-practices/communication-patterns/event-queue/

    public class Queue : ITest
    {
        //https://redis.io/commands/lpush
        //use async version
        async Task PutItemToQueue(string key, QueueItem item)
        {
            var db = DbFactory.GetDb();

            //your code here
            //..
        }

        //https://redis.io/commands/rpop
        //use async version
        async Task<QueueItem> GetItemFromQueue(string key)
        {
            var db = DbFactory.GetDb();

            //your code here
            //..

            return null;
        }

        //https://redis.io/commands/llen
        public long GetQueueSize(string key)
        {
            var db = DbFactory.GetDb();

            //your code here
            //..

            return 0;
        }
        public void RunTests()
        {
            Console.WriteLine("Queue test starts");

            int counter = 0;
            object _lock = new object();
            int total = 10000;
            string key = "my_queue";
            Utils.DeleteKey(key);
            var list = new List<QueueItem>();
            var tasks = new List<Task>();
            Enumerable.Range(0, total).ToList().ForEach(f =>
            {
                var t = Task.Run(async () =>
                {
                    var rg = new Random();
                    var action = rg.Next(1, 11);
                    if (action < 6)
                    {
                        lock (_lock)
                        {
                            counter++;
                        }
                        await PutItemToQueue(key, new QueueItem()
                        {
                            Id = counter,
                            Info = counter.ToString()
                        });
                    }
                    else
                    {
                        var item = await GetItemFromQueue(key);
                        if (item != null)
                        {
                            lock (_lock)
                            {
                                list.Add(item);
                            }
                        }
                    }
                });
                tasks.Add(t);
            });

            Task.WhenAll(tasks).Wait();

            if (!list.Any())
            {
                throw new Exception("bad queue");
            }

            var size = (int)GetQueueSize(key);
            for (int i = 0; i < size; i++)
            {
                list.Add(GetItemFromQueue(key).Result);
            }

            if (list.Count() != counter)
            {
                throw new Exception("bad queue");
            }

            Console.WriteLine("Queue test OK");
        }

    }

    public class QueueItem
    { 
        public int Id { get; set; }
        public string Info { get; set; }
    }
}
