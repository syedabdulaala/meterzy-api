using Meterzy.Entity.Data;
using Microsoft.EntityFrameworkCore;

namespace Meterzy.Data.Config
{
    internal sealed class FixedTariffConfig : TableConfig<FixedTariff>
    {
        public FixedTariffConfig(ModelBuilder modelBuilder) : base(modelBuilder)
        {
            Entity.HasKey(x => x.Id);
            Entity.Property(x => x.Charges).IsRequired(true);
            Entity.Property(x => x.Name).IsRequired(true);
            Entity.Property(x => x.TariffId).IsRequired(true);
            Entity.Property(x => x.UnitType).IsRequired(true);

            Entity.HasOne(x => x.Tariff)
                  .WithMany(x => x.FixedTariffs)
                  .HasForeignKey(x => x.TariffId)
                  .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
