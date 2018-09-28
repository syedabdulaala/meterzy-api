using Meterzy.Entity.Data;
using Microsoft.EntityFrameworkCore;

namespace Meterzy.Data.Config
{
    internal sealed class TariffConfig : TableConfig<Tariff>
    {
        internal TariffConfig(ModelBuilder modelBuilder) : base(modelBuilder)
        {
            Entity.HasKey(x => x.Id);
            Entity.Property(x => x.AppUserId).IsRequired(true);
            Entity.Property(x => x.Name).IsRequired(true);

            Entity.HasOne(x => x.AppUser)
                  .WithMany(x => x.Tariffs)
                  .HasForeignKey(x => x.AppUserId)
                  .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
