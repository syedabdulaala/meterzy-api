using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Meterzy.Api.Model.Request.Tariff
{
    public class UpdateTariffRequest : AddTariffRequest
    {
        [Required]
        public int Id { get; set; }
        public new UpdateFixedTariffRequest[] FixedTariffs { get; set; }
        public new UpdateRangedTariffRequest[] RangedTariffs { get; set; }
    }
}
