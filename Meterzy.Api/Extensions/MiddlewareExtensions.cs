using Meterzy.Api.Helpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net;

namespace Meterzy.Api.Extensions
{
    public static class MiddlewareExtensions
    {
        public static void UseGlobalExceptionHandler(this IApplicationBuilder appBuilder, ILogger logger)
        {
            appBuilder.UseExceptionHandler(app =>
            {
                app.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";

                    var ex = context.Features.Get<IExceptionHandlerFeature>()?.Error;
                    if (ex != null)
                    {
                        logger.LogError($"INTERNAL SERVER ERROR: {ex}");
                        var response = ErrorResponses.SomethingWentWrong;
                        response.Detail = Const.Environment == EnvironmentName.Development ? ex : null;
                        var serializedResponse = JsonConvert.SerializeObject(response);
                        await context.Response.WriteAsync(serializedResponse);
                    }
                });
            });
        }
    }
}
