using Meterzy.Entity.Data;
using Microsoft.EntityFrameworkCore;

namespace Meterzy.Data.Config
{
    internal sealed class MeterConfig : TableConfig<Meter>
    {
        internal MeterConfig(ModelBuilder modelBuilder) : base(modelBuilder)
        {
            Entity.HasKey(x => x.Id);
            Entity.Property(x => x.AccountNo).IsRequired(true);
            Entity.Property(x => x.AppUserId).IsRequired(true);
            Entity.Property(x => x.ConsumerNo).IsRequired(true);
            Entity.Property(x => x.Name).IsRequired(true);
            Entity.Property(x => x.TariffId).IsRequired(true);

            Entity.HasOne(x => x.AppUser)
                  .WithMany(x => x.Meters)
                  .HasForeignKey(x => x.AppUserId)
                  .OnDelete(DeleteBehavior.Restrict);
            Entity.HasOne(x => x.Tariff)
                  .WithMany(x => x.Meters)
                  .HasForeignKey(x => x.TariffId)
                  .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
