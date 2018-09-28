using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Meterzy.Api.Helpers.Models
{
    public class Secrets
    {
        //Property(ies)
        public string ConnectionString { get; set; }
        public string PasswordSalt { get; }
        public EncryptionKeys EncryptionKeys { get; }

        //Constructor(s)
        public Secrets(IConfiguration configuration)
        {
            ConnectionString = configuration[nameof(ConnectionString)];
            PasswordSalt = configuration[nameof(PasswordSalt)];
            EncryptionKeys = new EncryptionKeys(configuration.GetSection(nameof(EncryptionKeys)));
        }
    }

    public class EncryptionKeys
    {
        //Property(ies)
        public string Default { get; }
        public string Jwt { get; }

        //Constructor(s)
        public EncryptionKeys(IConfiguration configuration)
        {
            Default = configuration[nameof(Default)];
            Jwt = configuration[nameof(Jwt)];
        }
    }
}
