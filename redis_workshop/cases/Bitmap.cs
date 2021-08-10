using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace redis_workshop.cases
{
    //we need to store binary choice of millions of users
    //how to do that most efficiently?
    //using one bit for one user!
    public class Bitmap : ITest
    {
        //store binary choice using no more than 1 bit per user
        //https://redis.io/commands/setbit
        //hint: userId's are sequential, start with 0 and never change
        void StoreChoice(string key, int userId, bool choice)
        {
            var db = DbFactory.GetDb();

            //your code here
            //..
        }

        //https://redis.io/commands/getbit
        bool GetUsersChoice(string key, int userId)
        {
            var db = DbFactory.GetDb();

            //your code here
            //..

            return false;
        }

        public void RunTests()
        {
            Console.WriteLine("Bitmap test starts");
            var key = "userChoice";
            Utils.DeleteKey(key);
            Test(false, key);
            Utils.DeleteKey(key);
            Test(true, key);
            Console.WriteLine("Bitmap test OK");
        }

        void Test(bool reverse, string key)
        {
            int total = 1000;
            Enumerable.Range(0, total).ToList().ForEach(f =>
            {
                StoreChoice(key, f, (reverse ? f % 2 == 0 : f % 2 != 0));
            });
            var random = new Random();
            var userId = random.Next(0, total);
            var choice = GetUsersChoice(key, userId);

            if (choice != (reverse ? userId % 2 == 0 : userId % 2 != 0))
            {
                throw new Exception("bad GetUsersChoice");
            }
        }
    }
}
