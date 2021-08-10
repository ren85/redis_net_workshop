using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace redis_workshop.cases
{
    //you need to quickly test whether login name is unique
    //for small n HashSet is ok
    //for large n Bloom filter is a better choice
    //if Bloom filter says element is unique, it is
    //if Bloom filter says element is not unique - it may or may not be
    //but with good tuning of filter parameters false positives could be brought to small value 
    //and only few elements need to be tested for uniqueness using something else (maybe another Bloom filter, larger one)

    //implement simple Bloom filter
    public class BloomFilter : ITest
    {
        //https://redis.io/commands/getbit
        
        bool CheckElement(string key, string item, int total)
        {
            var bitToSet = HashFunction1(item, total);
            var db = DbFactory.GetDb();

            //your code here
            //..

            return false;
        }

        //https://redis.io/commands/setbit
        void StoreElement(string key, string item, int total)
        {
            var bitToSet = HashFunction1(item, total);
            var db = DbFactory.GetDb();

            //your code here
            //..
        }

        int HashFunction1(string name, int max)
        {
            var hash = name.GetHashCode();
            if (hash < 0)
            {
                return (int)(hash * (double)-154465457866654 % max);
            }
            else
            {
                return (int)(hash * (double)6546876164487 % max); 
            }
        }

        public void RunTests()
        {
            Console.WriteLine("Bloom test starts");
            var key = "bloomFilter";
            Utils.DeleteKey(key);

            var names = File.ReadAllLines(Path.Combine("cases", "female-names.txt")).Where(f => !string.IsNullOrEmpty(f?.Trim()) && !f.StartsWith("#"))
                            .Select(f => f.Trim().ToLower()).ToList();

            var bloomSize = names.Count() * 10;

            //with such size and one hash function there is around 10% collision probability, we neglect this here
            //https://hur.st/bloomfilter/

            names.ForEach(f => StoreElement(key, f, bloomSize));

            var check = CheckElement(key, "mike", bloomSize);
            if (check)
            {
                throw new Exception("bad Bloom filter");
            }

            check = CheckElement(key, "zoe", bloomSize);
            if (!check)
            {
                throw new Exception("bad Bloom filter");
            }

            Console.WriteLine("Bloom test OK");
        }
    }
}
