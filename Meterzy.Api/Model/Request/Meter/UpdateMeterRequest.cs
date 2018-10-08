using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Meterzy.Api.Model.Request.Meter
{
    public class UpdateMeterRequest : AddMeterRequest
    {
        public int Id { get; set; }
    }
}
