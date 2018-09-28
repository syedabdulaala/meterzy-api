using System;
using System.Collections.Generic;
using System.Text;

namespace Meterzy.Entity.Data
{
    public class Tariff : Table
    {
        public int Id { get; set; }
        public int AppUserId { get; set; }
        public string Name { get; set; }

        public AppUser AppUser { get; set; }
        public ICollection<Meter> Meters { get; set; }
        public ICollection<FixedTariff> FixedTariffs { get; set; }
        public ICollection<RangedTariff> RangedTariffs { get; set; }
    }
}
