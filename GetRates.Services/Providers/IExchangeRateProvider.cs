namespace GetRates.Services
{
    public interface IExchangeRateProvider
    {
        ReturnedData GetRate(decimal amount);
    }
}