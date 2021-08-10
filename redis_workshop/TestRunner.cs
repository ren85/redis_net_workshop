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
                new Leaderboard(),
                new Bitmap(),
                new Autocomplete(),
                new Counter(),
                new BloomFilter(),
                new Queue()
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
