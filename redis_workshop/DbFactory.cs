using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace redis_workshop
{
    public class DbFactory
    {
        static ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("127.0.0.1:6379");
        static object _lock = new object();
        static IDatabase _db { get; set; }
        public static IDatabase GetDb()
        {
            if (_db == null)
            {
                lock (_lock)
                {
                    if (_db == null)
                    {
                        _db = redis.GetDatabase();
                    }
                }
            }
            return _db;
        }
    }
}
