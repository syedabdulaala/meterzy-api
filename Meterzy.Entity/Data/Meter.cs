using System.Collections.Generic;

namespace Meterzy.Entity.Data
{
    public sealed class Meter : Table
    {
        public int Id { get; set; }
        public int TariffId { get; set; }
        public string Name { get; set; }
        public string AccountNo { get; set; }
        public string ConsumerNo { get; set; }
        public int AppUserId { get; set; }

        public AppUser AppUser { get; set; }
        public Tariff Tariff { get; set; }
        public ICollection<MeterReading> Readings { get; set; }
    }
}
