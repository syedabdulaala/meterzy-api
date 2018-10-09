using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Meterzy.Api.Model.Request.Tariff
{
    public class UpdateRangedTariffRequest : AddRangedTariffRequest
    {
        [Required]
        public int Id { get; set; }
    }
}
