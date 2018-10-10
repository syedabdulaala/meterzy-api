using Meterzy.Api.Controller.Tariff.Model;
using Meterzy.Api.Helper;
using Meterzy.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Meterzy.Api.Controller.Tariff
{
    [Route("tariff"), Authorize]
    public class TariffController : BaseApiController
    {
        #region Variable(s)
        private IRepo<Entity.Data.Tariff> _tariff;
        private IRepo<Entity.Data.FixedTariff> _fixedTariff;
        private IRepo<Entity.Data.RangedTariff> _rangedTariff;
        #endregion

        #region Constructor(s)
        public TariffController(IRepo<Entity.Data.Tariff> tariff, IRepo<Entity.Data.FixedTariff> fixedTariff, IRepo<Entity.Data.RangedTariff> rangedTariff, ILogger<BaseApiController> logger) : base(logger)
        {
            _tariff = tariff;
            _fixedTariff = fixedTariff;
            _rangedTariff = rangedTariff;
        }
        #endregion

        #region GET Method(s)
        [HttpGet, Route("/tariffs")]
        public async Task<IActionResult> All()
        {
            try
            {
                var tariffs = await _tariff.DataSet.Where(x => !x.Deleted && x.AppUserId == AuthenticatedUserId)
                                                   .Include(x => x.FixedTariffs)
                                                   .Include(x => x.RangedTariffs).ToListAsync();
                if (tariffs == null)
                {
                    return ClientError(
                        code: HttpResponse.TariffNotFound.Key,
                        message: HttpResponse.TariffNotFound.Value
                    );
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
                var exist = await _tariff.DataSet.Where(x => !x.Deleted && x.AppUserId == AuthenticatedUserId && x.Name.ToLower() == request.Name.Trim().ToLower()).AnyAsync();
                if (exist)
                {
                    return ClientError(
                        code: HttpResponse.TariffNotFound.Key,
                        message: HttpResponse.TariffNotFound.Value
                    );
                }

                var newTariff = new Entity.Data.Tariff()
                {
                    Name = request.Name,
                    AppUserId = AuthenticatedUserId,
                    FixedTariffs = request.FixedTariffs.Select(x => new Entity.Data.FixedTariff()
                    {
                        Name = x.Name,
                        Charges = x.Charges,
                        UnitType = (Entity.Data.TariffUnitType)x.UnitType
                    }).ToList(),
                    RangedTariffs = request.RangedTariffs.Select(x => new Entity.Data.RangedTariff()
                    {
                        Name = x.Name,
                        UpperRange = x.UpperRange,
                        LowerRange = x.LowerRange,
                        Charges = x.Charges,
                        UnitType = (Entity.Data.TariffUnitType)x.UnitType
                    }).ToList()
                };
                await _tariff.AddAsync(newTariff);
                if (await _tariff.SaveAsync() == 0)
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
        public async Task<IActionResult> Update([FromBody] UpdateTariffRequest request)
        {
            try
            {
                var tariff = await _tariff.DataSet.Where(x => !x.Deleted && x.AppUserId == AuthenticatedUserId)
                                                   .Include(x => x.RangedTariffs)
                                                   .Include(x => x.FixedTariffs).FirstOrDefaultAsync();
                if (tariff == null)
                {
                    return ClientError(
                        code: HttpResponse.TariffNotFound.Key,
                        message: HttpResponse.TariffNotFound.Value
                    );
                }

                tariff.Name = request.Name;
                foreach (var item in request.FixedTariffs)
                {
                    var updatedFixedTariff = tariff.FixedTariffs.Where(x => x.Id == item.Id).FirstOrDefault();
                    updatedFixedTariff.Name = item.Name;
                    updatedFixedTariff.Charges = item.Charges;
                }
                foreach (var item in request.RangedTariffs)
                {
                    var updatedRangedTariff = tariff.RangedTariffs.Where(x => x.Id == item.Id).FirstOrDefault();
                    updatedRangedTariff.Name = item.Name;
                    updatedRangedTariff.Charges = item.Charges;
                    updatedRangedTariff.LowerRange = item.LowerRange;
                    updatedRangedTariff.UpperRange = item.UpperRange;
                }
                await _tariff.UpdateAsync(tariff);
                var fixedTariffsToRemove = tariff.FixedTariffs.Where(x => !request.FixedTariffs.Select(y => y.Id).Contains(x.Id)).ToList();
                var rangedTariffsToRemove = tariff.RangedTariffs.Where(x => !request.RangedTariffs.Select(y => y.Id).Contains(x.Id)).ToList();
                foreach (var item in fixedTariffsToRemove)
                {
                    await _fixedTariff.SoftDeleteAsync(item);
                }
                foreach (var item in rangedTariffsToRemove)
                {
                    await _rangedTariff.SoftDeleteAsync(item);
                }

                if (await _tariff.SaveAsync() > 0)
                {
                    return ServerError(
                        code: HttpResponse.FailedToComplete.Key,
                        message: HttpResponse.FailedToComplete.Value
                    );
                }
                if (fixedTariffsToRemove.Count > 0)
                {
                    if (await _fixedTariff.SaveAsync() == 0)
                    {
                        return ServerError(
                            code: HttpResponse.FailedToComplete.Key,
                            message: HttpResponse.FailedToComplete.Value
                        );
                    }
                }
                if (rangedTariffsToRemove.Count > 0)
                {
                    if (await _rangedTariff.SaveAsync() == 0)
                    {
                        return ServerError(
                            code: HttpResponse.FailedToComplete.Key,
                            message: HttpResponse.FailedToComplete.Value
                        );
                    }
                }


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
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var tariff = await _tariff.DataSet.Where(x => !x.Deleted && x.AppUserId == AuthenticatedUserId && x.Id == id)
                                   .Include(x => x.RangedTariffs)
                                   .Include(x => x.FixedTariffs).FirstOrDefaultAsync();
                if (tariff == null)
                {
                    return ClientError(
                        code: HttpResponse.TariffNotFound.Key,
                        message: HttpResponse.TariffNotFound.Value
                    );
                }

                await _tariff.SoftDeleteAsync(tariff);
                if (await _tariff.SaveAsync() == 0)
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
    }
}
