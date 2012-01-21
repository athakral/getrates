namespace RateCheck.Services
{
    public interface IProviderService
    {
        ReturnedAmount GetRate(float amount);
    }

    public class ReturnedAmount
    {
        public string ProviderName { get; set; }
        public float ConversionRate { get; set; }
        public float ConvertedAmount { get; set; }
    }
}