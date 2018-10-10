using Meterzy.Api.Extension;
using Meterzy.Api.Helper;
using Meterzy.Api.Helper.Model;
using Meterzy.Data;
using Meterzy.Entity.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Linq;
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
            services.AddAuthentication(ConfigureAuthenticationOptions)
                    .AddJwtBearer(ConfigureJwtBearerOptions);
            services.AddScoped<IRepo<AppUser>>((x) => { return new Repo<AppUser>(x.GetService<MeterzyContext>()); });
            services.AddScoped<IRepo<Tariff>>((x) => { return new Repo<Tariff>(x.GetService<MeterzyContext>()); });
            services.AddScoped<IRepo<FixedTariff>>((x) => { return new Repo<FixedTariff>(x.GetService<MeterzyContext>()); });
            services.AddScoped<IRepo<RangedTariff>>((x) => { return new Repo<RangedTariff>(x.GetService<MeterzyContext>()); });
            services.AddScoped<IRepo<Meter>>((x) => { return new Repo<Meter>(x.GetService<MeterzyContext>()); });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseHsts();
            app.UseCors(x => x.AllowAnyOrigin()
                              .AllowAnyMethod()
                              .AllowAnyHeader()
                              .AllowCredentials());

            app.UseGlobalExceptionHandler(loggerFactory.CreateLogger(env.ApplicationName));
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseMvc();
        }

        private void InitConfig()
        {
            var configBuilder = new ConfigurationBuilder();
            configBuilder.SetBasePath(Const.Environment.ContentRootPath + "\\ConfigFile");
            configBuilder.AddJsonFile($"appsettings.{Const.Environment.EnvironmentName.ToLower()}.json", false, true)
                         .AddUserSecrets<Startup>();
            Config.Init(configBuilder.Build());
        }

        private void ConfigureMeterzyContextOptions(DbContextOptionsBuilder options)
        {
            options.UseSqlServer(Config.Secrets.ConnectionString);
        }

        private void ConfigureAuthenticationOptions(AuthenticationOptions options)
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }

        private void ConfigureJwtBearerOptions(JwtBearerOptions options)
        {
            options.Events = new JwtBearerEvents
            {
                OnTokenValidated = async context =>
                {
                    var appUser = context.HttpContext.RequestServices.GetRequiredService<IRepo<AppUser>>();
                    var userId = int.Parse(context.Principal.Identity.Name);
                    var user = await appUser.DataSet.Where(x => !x.Deleted && x.Id == userId).FirstOrDefaultAsync();
                    if (user == null)
                    {
                        context.Fail(Helper.HttpResponse.Unauthorized.Value);
                    }
                }
            };
            options.RequireHttpsMetadata = false;
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Config.Secrets.JwtKey)),
                ValidateIssuer = false,
                ValidateAudience = false
            };
        }
        #endregion
    }
}
