using System;
using System.Collections.Generic;

namespace Meterzy.Entity.Data
{
    public class MeterReading : Table
    {
        public int Id { get; set; }
        public int MeterId { get; set; }
        public int Reading { get; set; }
        public DateTime NotedOn { get; set; }

        public Meter Meter { get; set; }
    }
}
