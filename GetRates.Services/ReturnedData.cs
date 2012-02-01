namespace GetRates.Services
{
    public class ReturnedData
    {
        public string ProviderName { get; set; }
        public decimal Rate { get; set; }
        public decimal Fee { get; set; }
        public decimal Deductions { get; set; }
    }
}