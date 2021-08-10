using System;
using System.Collections.Generic;
using System.Text;

namespace redis_workshop.cases
{
    public class Utils
    {
        public static void DeleteKey(string key)
        {
            var db = DbFactory.GetDb();
            db.KeyDelete(key);
        }
    }
}
