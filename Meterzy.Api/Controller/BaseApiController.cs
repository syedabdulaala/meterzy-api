using Meterzy.Api.Helper;
using Meterzy.Api.Model.Response;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net;

namespace Meterzy.Api.Controller
{
    [ApiController]
    public class BaseApiController : ControllerBase
    {
        #region Variable(s)
        protected readonly ILogger _logger;
        protected int AuthenticatedUserId => int.Parse(User.Identity.Name);
        #endregion

        #region Constructor(s)
        public BaseApiController(ILogger<BaseApiController> logger)
        {
            _logger = logger;
        }
        #endregion

        #region Sync Method(s)
        protected IActionResult Success(object data = null, string code = null, string message = null, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            var response = new MetaResponse
            {
                Code = code ?? HttpResponse.Success.Key,
                Data = data,
                Message = message ?? HttpResponse.Success.Value
            };
            return StatusCode((int)statusCode, response);
        }

        protected IActionResult Redirect(object data = null, string code = null, string message = null, HttpStatusCode statusCode = HttpStatusCode.MultipleChoices)
        {
            var response = new MetaResponse
            {
                Code = code ?? HttpResponse.MutlpleChoices.Key,
                Data = data,
                Message = message ?? HttpResponse.MutlpleChoices.Value
            };
            return StatusCode((int)statusCode, response);
        }

        protected IActionResult ServerError(Exception ex = null, string code = null, string message = null, HttpStatusCode statusCode = HttpStatusCode.InternalServerError)
        {
            var response = new MetaResponse
            {
                Code = code ?? HttpResponse.SomethingWentWrong.Key,
                Data = Const.Environment.IsDevelopment() ? ex.ToString() : null,
                Message = message ?? HttpResponse.SomethingWentWrong.Value
            };
            return StatusCode((int)statusCode, response);
        }

        protected IActionResult ClientError(object data = null, string code = null, string message = null, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
        {
            var response = new MetaResponse
            {
                Code = code ?? HttpResponse.BadRequest.Key,
                Data = data,
                Message = message ?? HttpResponse.BadRequest.Value
            };
            return StatusCode((int)statusCode, response);
        }
        #endregion
    }
}
