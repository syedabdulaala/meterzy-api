﻿using Meterzy.Api.Helper;
using Meterzy.Api.Model.Request.Meter;
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
    [Route("meter"), Authorize]
    public class MeterController : BaseApiController
    {
        #region Variable(s)
        private readonly IRepo<Meter> _meter;
        #endregion

        #region Constructor(s)
        public MeterController(IRepo<Meter> meter, ILogger<BaseApiController> logger) : base(logger)
        {
            _meter = meter;
        }
        #endregion

        #region GET Method(s)
        [HttpGet, Route("/meters")]
        public async Task<IActionResult> All()
        {
            try
            {
                var meters = await _meter.DataSet.Where(x => x.AppUserId == loggedInUserId).Include(x => x.Tariff).ToListAsync();
                return Success(meters.Select(x => new
                {
                    x.Id,
                    x.AccountNo,
                    x.ConsumerNo,
                    x.Name,
                    Tariff = new
                    {
                        x.Tariff.Id,
                        x.Tariff.Name,
                    }
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
        public async Task<IActionResult> Add([FromBody] AddMeterRequest request)
        {
            try
            {
                var exist = await _meter.DataSet.Where(x => x.AccountNo == request.AccountNo).AnyAsync();
                if (exist)
                {
                    return ClientError(
                        code: HttpResponse.MeterAccountNoAlreadyInUse.Key,
                        message: HttpResponse.MeterAccountNoAlreadyInUse.Value
                    );
                }

                var newMeter = new Meter()
                {
                    AccountNo = request.AccountNo,
                    AppUserId = loggedInUserId,
                    ConsumerNo = request.ConsumerNo,
                    Name = request.Name,
                    TariffId = request.TariffId
                };
                await _meter.AddAsync(newMeter);
                await _meter.SaveAsync();
                return Success(new { newMeter.Id });
            }
            catch (Exception ex)
            {
                return ServerError(ex);
            }
        }
        #endregion

        #region PUT Method(s)
        [HttpPut, Route("update")]
        public async Task<IActionResult> Update([FromBody] UpdateMeterRequest request)
        {
            try
            {
                var meter = await _meter.DataSet.Where(x => !x.Deleted && x.Id == request.Id && x.AppUserId == loggedInUserId).FirstOrDefaultAsync();
                if (meter == null)
                {
                    return ClientError(
                        code: HttpResponse.MeterUnavailable.Key,
                        message: HttpResponse.MeterUnavailable.Value
                    );
                }

                var exist = await _meter.DataSet.Where(x => x.AccountNo == request.AccountNo).AnyAsync();
                if (exist)
                {
                    return ClientError(
                        code: HttpResponse.MeterAccountNoAlreadyInUse.Key,
                        message: HttpResponse.MeterAccountNoAlreadyInUse.Value
                    );
                }

                meter.AccountNo = request.AccountNo;
                meter.AppUserId = loggedInUserId;
                meter.ConsumerNo = request.ConsumerNo;
                meter.Name = request.Name;
                meter.TariffId = request.TariffId;
                await _meter.UpdateAsync(meter);
                if (await _meter.SaveAsync() == 0)
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

        #region DELETE Method(s)
        [HttpDelete, Route("delete")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var meter = await _meter.DataSet.Where(x => !x.Deleted && x.Id == id && x.AppUserId == loggedInUserId).FirstOrDefaultAsync();
                if (meter == null)
                {
                    return ClientError(
                        code: HttpResponse.MeterUnavailable.Key,
                        message: HttpResponse.MeterUnavailable.Value
                    );
                }

                await _meter.SoftDeleteAsync(meter);
                if (await _meter.SaveAsync() == 0)
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
