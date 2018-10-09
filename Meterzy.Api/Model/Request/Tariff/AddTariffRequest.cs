using System.ComponentModel.DataAnnotations;

namespace Meterzy.Api.Model.Request.Tariff
{
    public class AddTariffRequest
    {
        [Required]
        public string Name { get; set; }
        public AddFixedTariffRequest[] FixedTariffs { get; set; }
        public AddRangedTariffRequest[] RangedTariffs { get; set; }
    }
}
