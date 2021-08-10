using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace redis_workshop.cases
{   
    //implement leaderboard using redis sorted sets
    //leaderboard is a list of users sorted by their score

    public class Leaderboard : ITest
    {
        //implement using https://redis.io/commands/zadd
        void AddUpdateMember(string leaderboardName, string userName, int score)
        {
            var db = DbFactory.GetDb();
            
            //your code here
            //..
        }
        //implement using https://redis.io/commands/zrevrange
        List<string> GetTopN(string leaderboardName, int n)
        {
            var db = DbFactory.GetDb();

            //your code here
            //..

            return new List<string>();
        }
        //implement using https://redis.io/commands/zrank
        long? GetUsersRank(string leaderboardName, string userName)
        {
            var db = DbFactory.GetDb();

            //your code here
            //..

            return 0;
        }
        public void RunTests()
        {
            Console.WriteLine("Leaderboard test starts");
            string leaderboardName = "leaderboard";
            Utils.DeleteKey(leaderboardName);
            Test(false, leaderboardName);
            Utils.DeleteKey(leaderboardName);
            Test(true, leaderboardName);
            Console.WriteLine("Leaderboard test OK");
        }

        void Test(bool reverse, string leaderboardName)
        {
            int total = 1000;
            Enumerable.Range(0, total).ToList().ForEach(f =>
            {
                AddUpdateMember(leaderboardName, $"user_{(reverse ? total - f : f)}", f);
            });
            var random = new Random();
            var nr = random.Next(0, total);
            var rank = GetUsersRank(leaderboardName, $"user_{nr}");
            if (rank != (reverse  ? total - nr : nr))
            {
                throw new Exception("bad GetUsersRank");
            }

            int bestCount = 10;
            var top = GetTopN(leaderboardName, bestCount);
            for (int i = 0; i < bestCount; i++)
            {
                var userNr = Convert.ToInt32(top[i].Split("_".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)[1]);
                if (i != (reverse ? total - userNr : userNr))
                {
                    throw new Exception("bad GetUsersRank");
                }
            }
        }
    }
}
