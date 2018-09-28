using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Meterzy.Api.Controller
{
    [ApiController]
    public class BaseApiController : ControllerBase
    {
        #region Variable(s)
        protected readonly ILogger logger;
        #endregion

        #region Constructor(s)
        public BaseApiController(ILogger<BaseApiController> logger)
        {
            this.logger = logger;
        }
        #endregion
    }
}
