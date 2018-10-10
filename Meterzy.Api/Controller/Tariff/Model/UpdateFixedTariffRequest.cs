using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Meterzy.Api.Controller.Tariff.Model
{
    public class UpdateFixedTariffRequest : AddFixedTariffRequest
    {
        [Required]
        public int Id { get; set; }
    }
}
