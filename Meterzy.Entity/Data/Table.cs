using System;

namespace Meterzy.Entity.Data
{
    public abstract class Table
    {
        public bool Deleted { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? LastModifiedOn { get; set; }
    }
}
