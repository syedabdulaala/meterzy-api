using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Meterzy.Api.Model.Request.Tariff
{
    public class AddRangedTariffRequest
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public int UpperRange { get; set; }
        [Required]
        public int LowerRange { get; set; }
        [Required]
        public decimal Charges { get; set; }
        [Required, Range(0, 2)]
        public int UnitType { get; set; }
    }
}
