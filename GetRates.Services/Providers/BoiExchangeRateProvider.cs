using HtmlAgilityPack;
using System.Linq;

namespace GetRates.Services.Providers
{
    public class BoiExchangeRateProvider : IExchangeRateProvider
    {
        public ReturnedData GetRate(decimal amount)
        {
            var web = new HtmlWeb();
            var doc = web.Load("http://boiusa.com/ExchangeRate.aspx");

            var ratefromWeb = doc.DocumentNode.SelectNodes("//td").Skip(6).FirstOrDefault().InnerHtml;
                       
            var finalRate = (decimal)0.0;
            if (decimal.TryParse(ratefromWeb, out finalRate))
            {
                return new ReturnedData()
                {
                    ProviderName = "<a href=\"http://bankofindia.com/Nri.aspx\" target=\"_blank\">Bank of India</a>",
                    Rate = finalRate,
                    Fee = calculateFee(amount)
                };
            }
            return null;

        }

        private decimal calculateFee(decimal amount)
        {
            return (decimal)2.50;
        }
    }
}