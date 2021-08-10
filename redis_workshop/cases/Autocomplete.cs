using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace redis_workshop.cases
{
    //implement simple autocomplete of ascii-character words
    //must be better than O(n)

    //hint: https://gist.github.com/antirez/11126283
    class Autocomplete : ITest
    {
        //implement using https://redis.io/commands/zadd
        void AddName(string key, string name)
        {
            var db = DbFactory.GetDb();
            
            //your code here
            //..
        }
        //implement using https://redis.io/commands/zrange (with BYLEX option)
        List<string> GetCompletions(string key, string input, int n)
        {
            var db = DbFactory.GetDb();

            //your code here
            //..

            return new List<string>();
        }

        public void RunTests()
        {
            Console.WriteLine("Autocomplete test starts");
            string key = "name";
            Utils.DeleteKey(key);
            File.ReadAllLines(Path.Combine("cases", "female-names.txt")).Where(f => !string.IsNullOrEmpty(f?.Trim()) && !f.StartsWith("#")).Select(f => f.Trim().ToLower())
                .ToList().ForEach(f => AddName(key, f));

            var completions = GetCompletions(key, "aa", 10);
            if (completions.Aggregate((a,b) => a + ", " + b) != "aaren, aarika")
            {
                throw new Exception("bad GetCompletions");
            }

            completions = GetCompletions(key, "jan", 5);
            if (completions.Aggregate((a, b) => a + ", " + b) != "jan, jana, janaya, janaye, jandy")
            {
                throw new Exception("bad GetCompletions");
            }

            Console.WriteLine("Autocomplete test OK");
        }
    }
}


