using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Meterzy.Api.Helper
{
    public static class Const
    {
        public static IHostingEnvironment Environment { get; set; }

        public static class HttpHeaderKey
        {
            public const string Authorization = nameof(Authorization);
        }
    }
}
