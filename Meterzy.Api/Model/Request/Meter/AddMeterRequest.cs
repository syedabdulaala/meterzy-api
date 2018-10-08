using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Meterzy.Api.Model.Request.Meter
{
    public class AddMeterRequest
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string AccountNo { get; set; }
        [Required]
        public string ConsumerNo { get; set; }
        [Required]
        public int TariffId { get; set; }
    }
}
