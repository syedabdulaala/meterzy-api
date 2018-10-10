using Humanizer;
using Meterzy.Entity.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace Meterzy.Api.Controller.Literal
{
    [Route("literal"), Authorize]
    public class LiteralController : BaseApiController
    {
        public LiteralController(ILogger<BaseApiController> logger) : base(logger)
        {
        }

        [HttpGet, Route("/literals"), ResponseCache(NoStore = false, Duration = 10000)]
        public IActionResult All()
        {
            try
            {
                var userStatuses = Enum.GetValues(typeof(AppUserStatus)).Cast<AppUserStatus>().Select(x => new
                {
                    Id = (int)x,
                    Name = x.ToString().Humanize(LetterCasing.Sentence)
                });
                var tariffUnitTypes = Enum.GetValues(typeof(TariffUnitType)).Cast<TariffUnitType>().Select(x => new
                {
                    Id = (int)x,
                    Name = x.ToString().Humanize(LetterCasing.Sentence)
                });
                return Success(new
                {
                    userStatuses,
                    tariffUnitTypes
                });
            }
            catch (Exception ex)
            {
                return ServerError(ex);
            }
        }
    }
}
