﻿using Meterzy.Api.Helpers.Models;
using Microsoft.Extensions.Configuration;

namespace Meterzy.Api.Helpers
{
    public static class Config
    {
        //Property(ies)
        public static AppSettings AppSettings { get; private set; }
        public static Secrets Secrets { get; private set; }

        //Method(s)
        public static void Init(IConfiguration configuration)
        {
            Secrets = new Secrets(configuration);
            AppSettings = new AppSettings(configuration);
        }
    }
}
