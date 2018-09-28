using Meterzy.Entity.Data;
using Microsoft.EntityFrameworkCore;

namespace Meterzy.Data.Config
{
    internal sealed class RangedTariffConfig : TableConfig<RangedTariff>
    {
        public RangedTariffConfig(ModelBuilder modelBuilder) : base(modelBuilder)
        {
            Entity.HasKey(x => x.Id);
            Entity.Property(x => x.Charges).IsRequired(true);
            Entity.Property(x => x.LowerRange).IsRequired(true);
            Entity.Property(x => x.Name).IsRequired(true);
            Entity.Property(x => x.TariffId).IsRequired(true);
            Entity.Property(x => x.UnitType).IsRequired(true);
            Entity.Property(x => x.UpperRange).IsRequired(true);

            Entity.HasOne(x => x.Tariff)
                  .WithMany(x => x.RangedTariffs)
                  .HasForeignKey(x => x.TariffId)
                  .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
