using System.ComponentModel.DataAnnotations;

namespace Meterzy.Api.Controller.Tariff.Model
{
    public class AddTariffRequest
    {
        [Required]
        public string Name { get; set; }
        public AddFixedTariffRequest[] FixedTariffs { get; set; }
        public AddRangedTariffRequest[] RangedTariffs { get; set; }
    }
}
