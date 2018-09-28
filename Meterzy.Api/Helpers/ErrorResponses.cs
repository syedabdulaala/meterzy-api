using Meterzy.Api.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Meterzy.Api.Helpers
{
    public static class ErrorResponses
    {
        #region 100's Informational
        public static readonly ErrorResponse Processing = new ErrorResponse("100.1", "Server is processing your request. It may take several minutes."),
        #endregion

        #region 200's Success

        #endregion

        #region 300's Redirection

        #endregion

        #region 400's Client error

        #endregion

        #region 500's Server error
            SomethingWentWrong = new ErrorResponse("500.1", "Oops! Something went wrong on our side");
        #endregion
    }
}
