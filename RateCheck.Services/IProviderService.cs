namespace RateCheck.Services
{
    public interface IProviderService
    {
        ReturnedData GetRate(decimal amount);
    }

    public class ReturnedData
    {
        public string ProviderName { get; set; }
        public decimal Rate { get; set; }
        public decimal Fee { get; set; }
        public decimal Deductions { get; set; }
    }
}