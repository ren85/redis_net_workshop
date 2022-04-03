using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace redis_workshop.cases
{
    //implement ditributed lock (i.e. inter-processes lock)
    //besides atomic locking we need to account for 2 additional problems
    // - how to release a lock if process crashes (answer: key expiration)
    // - when using key expiration to solve problem above what to do when critical section takes longer than the expiration period (answer: special worker thread to renew expiration period)
    public class DistributedLock : ITest
    {
        public void RunTests()
        {
            Console.WriteLine("DistributedLock test starts");
            var key = "dlock";
            Utils.DeleteKey(key);
            Process.SetharedCounter(0);
            long sharedCounterTest = 0;
            object sharedCounterTestLock = new object();

            var processes = Enumerable.Range(0, 100).ToList().Select(f => new Process()).ToList();
            Parallel.ForEach(processes, p =>
            {
                var res = p.CriticalSection();
                lock (sharedCounterTestLock)
                {
                    sharedCounterTest += res;
                }
            });

            var realCounter = Process.GetSharedCounter();
            if (sharedCounterTest == realCounter)
            {
                Console.WriteLine("DistributedLock test OK");
            }
            else
            {
                throw new Exception("bad DistributedLock");
            }
        }
    }

    public class Process
    {
        static string KeyName = "dlock";
        static int expirationMs = 2000;
        static TimeSpan expiration = new TimeSpan(0, 0, 0, 0, expirationMs);

        static long sharedCounter = 0;

        bool criticalSection = false;
        object criticalSectionLock = new object();
        void AquireDistributedLock()
        {
            var db = DbFactory.GetDb();
            var isSet = false;
            while (!isSet)
            {
                Thread.Sleep(expirationMs / 10);
                //implement using https://redis.io/commands/set/ 
                //your code here
                //..
                //isSet = ..
            }
            criticalSection = true;
            Task.Run(() =>
            {
                while (criticalSection)
                {
                    Thread.Sleep(expirationMs / 10);
                    lock (criticalSectionLock)
                    {
                        if (criticalSection)
                        {
                            //to simplify - this never fails
                            //also we need to account for network delay: is expirationMs is smaller than delay this is not going to work, we set expirationMs large enough

                            //implement using https://redis.io/commands/set/ 
                            //your code here
                            //..
                        }
                    }
                }
            });
        }
        void ReleaseDistributedLock()
        {
            Utils.DeleteKey(KeyName);
            lock (criticalSectionLock)
            {
                criticalSection = false;
            }
        }
        public int CriticalSection()
        {
            try
            {
                AquireDistributedLock();
                var rg = new Random();

                //to make it sensitive to race conditions bugs
                var oldvalue = sharedCounter;
                sharedCounter = 0;
                Thread.Sleep(rg.Next(0, 1000));

                var random = rg.Next(0, 100);
                var sign = rg.Next(0, 10) > 5 ? 1 : -1;
                sharedCounter = oldvalue + sign * random;
                return sign * random;
            }
            finally
            {
                ReleaseDistributedLock();
            }
        }

        public static long GetSharedCounter()
        {
            return sharedCounter;
        }
        public static void SetharedCounter(int value)
        {
            sharedCounter = value;
        }
    }
}
