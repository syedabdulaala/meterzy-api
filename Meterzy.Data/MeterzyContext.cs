using JetBrains.Annotations;
using Meterzy.Data.Config;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Meterzy.Data
{
    public class MeterzyContext : DbContext
    {
        public MeterzyContext(DbContextOptions options) : base(options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            new AppUserConfig(modelBuilder);
            new MeterConfig(modelBuilder);
            new TariffConfig(modelBuilder);
            new FixedTariffConfig(modelBuilder);
            new RangedTariffConfig(modelBuilder);
            new MeterReadingConfig(modelBuilder);
            base.OnModelCreating(modelBuilder);
        }
    }
}
