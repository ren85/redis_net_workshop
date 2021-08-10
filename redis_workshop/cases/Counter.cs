using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace redis_workshop.cases
{
    //implement thread-safe counter
    public class Counter : ITest
    {
        //implement using https://redis.io/commands/incrby
        //use async version
        async Task Increment(string key, int increment)
        {
            var db = DbFactory.GetDb();

            //your code here
            //..
        }
        //implement using https://redis.io/commands/get
        long GetCounter(string key)
        {
            var db = DbFactory.GetDb();

            //your code here
            //..

            return 0;
        }

        public void RunTests()
        {
            Console.WriteLine("Increment test starts");
            string key = "counter";
            Utils.DeleteKey(key);
            long test_count = 0;
            object _lock = new object();
            var tasks = new List<Task>();
            Enumerable.Range(0, 100).ToList().ForEach(f =>
            {
                var t = Task.Run(async () =>
                {
                    var rg = new Random();
                    var increment = rg.Next(0, 5000);
                    increment *= (rg.Next(1, 11) > 5 ? -1 : 1);
                    await Increment(key, increment);
                    lock (_lock)
                    {
                        test_count += increment;
                    }
                });
                tasks.Add(t);
            });

            Task.WhenAll(tasks).Wait();

            var db = DbFactory.GetDb();

            if (test_count != GetCounter(key))
            {
                throw new Exception("bad Increment");
            }

            Console.WriteLine("Increment test OK");
        }
    }
}
