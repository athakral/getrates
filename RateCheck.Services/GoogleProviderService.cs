using HtmlAgilityPack;

namespace RateCheck.Services
{
    public class GoogleProviderService : IProviderService
    {
        public ReturnedAmount GetRate(float amount)
        {
            var web = new HtmlWeb();
            var doc = web.Load("http://www.google.com/ig/calculator?hl=en&q=1USD=?INR");
            var innerText = doc.DocumentNode.InnerText;
            var ratefromWeb = innerText.Split(',')[1].Split(':')[1].Split(' ')[1].Replace("\"","");
            var finalRate = (float)0.0;
            if (float.TryParse(ratefromWeb, out finalRate))
            {
                return new ReturnedAmount()
                           {
                               ProviderName = "Google",
                               ConversionRate = finalRate,
                               ConvertedAmount = amount*finalRate
                           };
            }
            return null;
        }
    }
}