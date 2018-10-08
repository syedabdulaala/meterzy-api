using Meterzy.Api.Extension;
using Meterzy.Api.Helper;
using Meterzy.Data;
using Meterzy.Entity.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

namespace Meterzy.Api
{
    public class Startup
    {
        #region Variable(s)
        private static ILoggerFactory loggerFactory;
        #endregion

        #region Constructor(s)
        public Startup(ILoggerFactory loggerFactory, IHostingEnvironment env)
        {
            Startup.loggerFactory = loggerFactory;
            Const.Environment = env;
            InitConfig();
        }
        #endregion

        #region Sync Method(s)
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddDbContext<MeterzyContext>(ConfigureMeterzyContextOptions, ServiceLifetime.Scoped, ServiceLifetime.Scoped);
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(ConfigureJwtBearerOptions);
            services.AddScoped<IRepo<AppUser>>((x) => { return new Repo<AppUser>(x.GetService<MeterzyContext>()); });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseHsts();
            app.UseCors();
            app.UseGlobalExceptionHandler(loggerFactory.CreateLogger("GlobalExceptionHandler"));
            app.UseHttpsRedirection();
            app.UseMvc();
        }

        private void InitConfig()
        {
            var configBuilder = new ConfigurationBuilder();
            configBuilder.SetBasePath(Const.Environment.ContentRootPath + "\\ConfigFile");
            var secretsJson = Environment.GetEnvironmentVariable("Meterzy_Secrets", EnvironmentVariableTarget.User);
            var memoryFileProvider = new InMemoryFileProvider(secretsJson);
            configBuilder.AddJsonFile($"appsettings.{Const.Environment.EnvironmentName.ToLower()}.json", false, true)
                         .AddJsonFile(memoryFileProvider, "secrets.json", false, false);
            Config.Init(configBuilder.Build());
        }

        private void ConfigureMeterzyContextOptions(DbContextOptionsBuilder options)
        {
            options.UseSqlServer(Environment.GetEnvironmentVariable("Meterzy_ConnStr", EnvironmentVariableTarget.User));
        }

        private void ConfigureJwtBearerOptions(JwtBearerOptions options)
        {
            options.Authority = Config.AppSettings.Jwt.Authority;
            options.Audience = Config.AppSettings.Jwt.Audience;
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                RequireSignedTokens = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidIssuer = Config.AppSettings.Jwt.Authority,
                ValidAudience = Config.AppSettings.Jwt.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Config.Secrets.EncryptionKeys.Jwt))
            };
        }
        #endregion
    }
}
