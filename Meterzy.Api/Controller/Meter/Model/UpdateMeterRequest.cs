using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Meterzy.Api.Controller.Meter.Model
{
    public class UpdateMeterRequest : AddMeterRequest
    {
        public int Id { get; set; }
    }
}
