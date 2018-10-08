namespace Meterzy.Api.Model.Request.Tariff
{
    public class AddTariffRequest
    {
        public string Name { get; set; }
        public AddFixedTariffRequest[] FixedTariffs { get; set; }
        public AddRangedTariffRequest[] RangedTariff { get; set; }
    }

    public class AddFixedTariffRequest
    {
        public string Name { get; set; }
        public decimal Charges { get; set; }
        public int UnitType { get; set; }
    }

    public class AddRangedTariffRequest
    {
        public string Name { get; set; }
        public int UpperRange { get; set; }
        public int LowerRange { get; set; }
        public decimal Charges { get; set; }
        public int UnitType { get; set; }
    }
}
