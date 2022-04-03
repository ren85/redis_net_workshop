using redis_workshop.cases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace redis_workshop
{
    public class TestRunner
    {
        public static void Run()
        {
            var list = new List<ITest>()
            {
                new Counter(),
                new Bitmap(),
                new Leaderboard(),
                new Queue(),
                new BloomFilter(),
                new DistributedLock(),
                new Autocomplete()
            };
            foreach (var l in list)
            {
                l.RunTests();
            }

            for (int i = 0; i < 3; i++)
            {
                Console.WriteLine($"SHUFFLE {i}");
                list = list.OrderBy(f => Guid.NewGuid()).ToList();
                foreach (var l in list)
                {
                    l.RunTests();
                }
            }
        }
    }
}
