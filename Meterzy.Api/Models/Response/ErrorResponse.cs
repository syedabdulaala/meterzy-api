using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Meterzy.Api.Models.Response
{
    public class ErrorResponse
    {
        #region Constructor(s)
        public ErrorResponse()
        {

        }

        public ErrorResponse(string code, string message)
        {
            Code = code;
            Message = message;
        }

        #endregion

        #region Property(ies)
        public string Code { get; set; }
        public string Message { get; set; }
        public object Detail { get; set; }
        #endregion        
    }
}
