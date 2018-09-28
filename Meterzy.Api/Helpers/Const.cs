using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Meterzy.Api.Helpers
{
    public static class Const
    {
        public static string ApplicationName { get; set; }
        public static string Environment { get; set; }

        public static class Paths
        {
            public static string ContentRoot { get; set; }
            public static string WebRoot { get; set; }
        }
    }
}
