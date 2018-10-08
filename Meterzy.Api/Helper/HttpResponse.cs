using System.Collections.Generic;

namespace Meterzy.Api.Helper
{
    public static class HttpResponse
    {
        #region 100's Informational
        public static readonly KeyValuePair<string, string> Processing = new KeyValuePair<string, string>("100.1", "Server is processing your request. It may take several minutes."),
        #endregion

        #region 200's Success
            Success = new KeyValuePair<string, string>("200.1", "Server successfully completed you request."),
        #endregion

        #region 300's Redirection
            MutlpleChoices = new KeyValuePair<string, string>("400.1", "Server has more than one possible responses."),
        #endregion

        #region 400's Client error
            BadRequest = new KeyValuePair<string, string>("400.1", "Server failed to validate your request data."),
            InvalidCredentials = new KeyValuePair<string, string>("400.2", "Invalid credentials."),
            EmailAddressAlreadyExist = new KeyValuePair<string, string>("400.3", "Email address already exist."),
            MeterAccountNoAlreadyInUse = new KeyValuePair<string, string>("400.4", "Account no is already is use."),
            MeterUnavailable = new KeyValuePair<string, string>("400.5", "Meter(s) not available."),
        #endregion

        #region 500's Server error
            SomethingWentWrong = new KeyValuePair<string, string>("500.1", "Oops! Something went wrong on our server."),
            FailedToComplete = new KeyValuePair<string, string>("500.2", "Server failed to complete your request.");
        #endregion
    }
}
