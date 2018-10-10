using Microsoft.Extensions.Configuration;

namespace Meterzy.Api.Helper.Model
{
    public class Secrets
    {
        #region Property(ies)
        public string ConnectionString { get; }
        public string Salt { get; }
        public string JwtKey { get; }
        #endregion

        #region Constructor(s)
        public Secrets(IConfiguration configuration)
        {
            ConnectionString = configuration[nameof(ConnectionString)];
            Salt = configuration[nameof(Salt)];
            JwtKey = configuration[nameof(JwtKey)];
        }
        #endregion        
    }
}
