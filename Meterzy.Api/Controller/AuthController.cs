using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Meterzy.Api.Controller
{
    public class AuthController : BaseApiController
    {
        public AuthController(ILogger<BaseApiController> logger) : base(logger)
        {
        }
    }
}