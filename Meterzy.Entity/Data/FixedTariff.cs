using System;
using System.Collections.Generic;
using System.Text;

namespace Meterzy.Entity.Data
{
    public sealed class FixedTariff : Table
    {
        public int Id { get; set; }
        public int TariffId { get; set; }
        public string Name { get; set; }
        public decimal Charges { get; set; }
        public TariffUnitType UnitType { get; set; }

        public Tariff Tariff { get; set; }
    }
}
