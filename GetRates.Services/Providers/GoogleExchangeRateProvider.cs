using HtmlAgilityPack;

namespace GetRates.Services.Providers
{
    public class GoogleExchangeRateProvider : IExchangeRateProvider
    {
        public ReturnedData GetRate(decimal amount)
        {
            var web = new HtmlWeb();
            var doc = web.Load("http://www.google.com/ig/calculator?hl=en&q=1USD=?INR");
            var innerText = doc.DocumentNode.InnerText;
            var ratefromWeb = innerText.Split(',')[1].Split(':')[1].Split(' ')[1].Replace("\"","");
            var finalRate = (decimal)0.0;
            if (decimal.TryParse(ratefromWeb, out finalRate))
            {
                return new ReturnedData()
                           {
                               ProviderName = "Google",
                               Rate = finalRate
                           };
            }
            return null;
        }
    }
}