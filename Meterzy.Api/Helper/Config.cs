using Meterzy.Api.Helper.Models;
using Microsoft.Extensions.Configuration;

namespace Meterzy.Api.Helper
{
    public static class Config
    {
        #region Property(ies)
        public static AppSettings AppSettings { get; private set; }
        public static Secrets Secrets { get; private set; }
        #endregion

        #region Constructor(s)
        public static void Init(IConfiguration configuration)
        {
            Secrets = new Secrets(configuration);
            AppSettings = new AppSettings(configuration);
        }
        #endregion
    }
}
