using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Data
{
    public class DataSource
    {
        public static Dictionary<string, string> Users = new Dictionary<string, string>()
        {
            { "admin", "password" },
            { "user", "password" }
        };
    }
}