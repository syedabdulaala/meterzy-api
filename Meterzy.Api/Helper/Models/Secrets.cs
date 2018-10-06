using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Meterzy.Api.Helper.Models
{
    public class Secrets
    {
        #region Property(ies)
        public string PasswordSalt { get; }
        public EncryptionKeys EncryptionKeys { get; }
        #endregion

        #region Constructor(s)
        public Secrets(IConfiguration configuration)
        {
            PasswordSalt = configuration[nameof(PasswordSalt)];
            EncryptionKeys = new EncryptionKeys(configuration.GetSection(nameof(EncryptionKeys)));
        }
        #endregion        
    }

    public class EncryptionKeys
    {
        #region Property(ies)
        public string Default { get; }
        public string Jwt { get; }
        #endregion

        #region Constructor(s)
        public EncryptionKeys(IConfiguration configuration)
        {
            Default = configuration[nameof(Default)];
            Jwt = configuration[nameof(Jwt)];
        }
        #endregion
    }
}
