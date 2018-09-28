using Meterzy.Api.Helpers;
using Meterzy.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Meterzy.Api
{
    public class Startup
    {
        #region Variable(s)
        private static ILoggerFactory loggerFactory;
        #endregion

        #region Constructor(s)
        public Startup(ILoggerFactory loggerFactory)
        {
            Startup.loggerFactory = loggerFactory;
        }
        #endregion

        #region Sync Method(s)
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddDbContext<MeterzyContext>((options) => { options.UseSqlServer(Config.Secrets.ConnectionString); }, ServiceLifetime.Scoped, ServiceLifetime.Scoped);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvcWithDefaultRoute();
        }
        #endregion

    }
}
