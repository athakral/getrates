using GetRates.Services;

namespace GetRates.Web
{
    public class CalculatedData : ReturnedData
    {
        public CalculatedData(ReturnedData baseData)
        {
            this.Rate = baseData.Rate;
            this.ProviderName = baseData.ProviderName;
            this.Fee = baseData.Fee;
            this.Deductions = baseData.Deductions;
        }
        public decimal AmountWithdrawn { get; set; }
        public decimal Amount { get; set; }
        public decimal EffectiveRate { get; set; }
        
    }
}