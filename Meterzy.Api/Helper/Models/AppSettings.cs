﻿using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Meterzy.Api.Helper.Models
{
    public class AppSettings
    {
        #region Property(ies)
        public Jwt Jwt { get; }
        #endregion

        #region Constructor(s)
        public AppSettings(IConfiguration configuration)
        {
            Jwt = new Jwt(configuration.GetSection(nameof(Jwt)));
        }
        #endregion
    }

    public class Jwt
    {
        #region Property(ies)
        public string Authority { get; }
        public string Audience { get; }
        #endregion

        #region Constructor(s)
        public Jwt(IConfiguration configuration)
        {
            Authority = configuration[nameof(Authority)];
            Audience = configuration[nameof(Authority)];
        }
        #endregion
    }
}
