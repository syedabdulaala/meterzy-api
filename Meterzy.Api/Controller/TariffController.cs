using Meterzy.Data;
using Meterzy.Entity.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Meterzy.Api.Controller
{
    [Route("tariff"), Authorize]
    public class TariffController : BaseApiController
    {
        #region Variable(s)
        private IRepo<Tariff> _tariff;
        #endregion

        #region Constructor(s)
        public TariffController(IRepo<Tariff> tariff, ILogger<BaseApiController> logger) : base(logger)
        {
            _tariff = tariff;
        }
        #endregion

        #region GET Method(s)
        [HttpGet, Route("/tariffs")]
        public async Task<IActionResult> All()
        {
            try
            {
                var tariffs = await _tariff.DataSet.Where(x => !x.Deleted && x.AppUserId == loggedInUserId)
                                                   .Include(x => x.FixedTariffs)
                                                   .Include(x => x.RangedTariffs).ToListAsync();
                if (tariffs == null)
                {
                    return ClientError();
                }

                return Success(tariffs.Select(x => new
                {
                    x.Id,
                    x.Name,
                    FixedTariffs = x.FixedTariffs.Select(y => new { y.Id, y.Name }),
                    RangedTariffs = x.RangedTariffs.Select(y => new { y.Id, y.Name })
                }));
            }
            catch (Exception ex)
            {
                return ServerError(ex);
            }
        }
        #endregion

        #region POST Method(s)
        [HttpPost, Route("add")]
        public async Task<IActionResult> Add()
        {
            try
            {
                return Success();
            }
            catch (Exception ex)
            {
                return ServerError(ex);
            }
        }
        #endregion

        #region PUT Method(s)
        [HttpPut, Route("update")]
        public async Task<IActionResult> Update()
        {
            try
            {
                return Success();
            }
            catch (Exception ex)
            {
                return ServerError(ex);
            }
        }
        #endregion

        #region DELETE Method(s)
        [HttpDelete, Route("delete")]
        public async Task<IActionResult> Delete()
        {
            try
            {
                return Success();
            }
            catch (Exception ex)
            {
                return ServerError(ex);
            }
        }
        #endregion
    }
}
