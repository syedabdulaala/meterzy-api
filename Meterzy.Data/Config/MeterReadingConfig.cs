using Meterzy.Entity.Data;
using Microsoft.EntityFrameworkCore;

namespace Meterzy.Data.Config
{
    internal sealed class MeterReadingConfig : TableConfig<MeterReading>
    {
        public MeterReadingConfig(ModelBuilder modelBuilder) : base(modelBuilder)
        {
            Entity.HasKey(x => x.Id);
            Entity.Property(x => x.MeterId).IsRequired(true);
            Entity.Property(x => x.NotedOn).IsRequired(true);
            Entity.Property(x => x.Reading).IsRequired(true);

            Entity.HasOne(x => x.Meter)
                  .WithMany(x => x.Readings)
                  .HasForeignKey(x => x.MeterId)
                  .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
