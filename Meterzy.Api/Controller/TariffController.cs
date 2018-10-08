using Meterzy.Api.Helper;
using Meterzy.Api.Model.Request.Tariff;
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
        private IRepo<FixedTariff> _fixedTariff;
        private IRepo<RangedTariff> _rangedTariff;
        #endregion

        #region Constructor(s)
        public TariffController(IRepo<Tariff> tariff, IRepo<FixedTariff> fixedTariff, IRepo<RangedTariff> rangedTariff, ILogger<BaseApiController> logger) : base(logger)
        {
            _tariff = tariff;
            _fixedTariff = fixedTariff;
            _rangedTariff = rangedTariff
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
        public async Task<IActionResult> Add([FromBody] AddTariffRequest request)
        {
            try
            {
                var exist = await _tariff.DataSet.Where(x => !x.Deleted && x.AppUserId == loggedInUserId && x.Name.ToLower() == request.Name.Trim().ToLower()).AnyAsync();
                if (exist)
                {

                }

                var newTariff = new Tariff()
                {
                    Name = request.Name,
                    AppUserId = loggedInUserId
                };
                await _tariff.AddAsync(newTariff);
                if (await _tariff.SaveAsync() == 0)
                {
                    return ServerError(
                        code: HttpResponse.FailedToComplete.Key,
                        message: HttpResponse.FailedToComplete.Value
                    );
                }

                var newFixedTariffs = request.FixedTariffs.Select(x => new FixedTariff()
                {
                    TariffId = newTariff.Id,
                    Name = x.Name,
                    Charges = x.Charges,
                    UnitType = (TariffUnitType)x.UnitType
                });
                foreach (var item in newFixedTariffs)
                {
                    await _fixedTariff.AddAsync(item);
                }
                if (await _fixedTariff.SaveAsync() == 0)
                {
                    return ServerError(
                        code: HttpResponse.FailedToComplete.Key,
                        message: HttpResponse.FailedToComplete.Value
                    );
                }

                var newRangedTariffs = request.RangedTariff.Select(x => new RangedTariff()
                {
                    TariffId = newTariff.Id,
                    Name = x.Name,
                    UpperRange = x.UpperRange,
                    LowerRange = x.LowerRange,
                    Charges = x.Charges,
                    UnitType = (TariffUnitType)x.UnitType
                });
                foreach (var item in newRangedTariffs)
                {
                    await _rangedTariff.AddAsync(item);
                }
                if (await _rangedTariff.SaveAsync() == 0)
                {
                    return ServerError(
                        code: HttpResponse.FailedToComplete.Key,
                        message: HttpResponse.FailedToComplete.Value
                    );
                }

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
