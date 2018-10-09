using Meterzy.Api.Extension;
using Meterzy.Api.Helper;
using Meterzy.Data;
using Meterzy.Entity.Data;
using Microsoft.AspNetCore.Authentication;
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
using System.Linq;
using System.Threading.Tasks;

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

            app.UseGlobalExceptionHandler(loggerFactory.CreateLogger("GlobalExceptionHandler"));
            app.UseHttpsRedirection();
            app.UseAuthentication();
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
                        context.Fail("Unauthorized");
                    }
                }
            };
            options.RequireHttpsMetadata = false;
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Config.Secrets.EncryptionKeys.Jwt)),
                ValidateIssuer = false,
                ValidateAudience = false
            };
        }
        #endregion
    }
}
