using System;
using System.Collections.Generic;
using System.Text;

namespace Meterzy.Entity.Data
{
    public enum AppUserStatus
    {
        Active,
        TemporarySuspended,
        PermanentlySuspended
    }

    public enum TariffType
    {
        Fixed,
        Ranged
    }

    public enum TariffUnitType
    {
        Raw,
        Percent
    }
}
