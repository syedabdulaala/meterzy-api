using System;
using System.Collections.Generic;
using System.Text;

namespace Meterzy.Entity.Data
{
    public sealed class AppUser : Table
    {
        public int Id { get; set; }
        public string EmailHash { get; set; }
        public string PasswordHash { get; set; }
        public string DisplayName { get; set; }
        public AppUserStatus Status { get; set; }

        public ICollection<Meter> Meters { get; set; }
        public ICollection<Tariff> Tariffs { get; set; }
    }
}
