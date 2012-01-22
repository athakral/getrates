using HtmlAgilityPack;

namespace RateCheck.Services
{
    public class XoomProviderService : IProviderService
    {
        public ReturnedData GetRate(decimal amount)
        {
            var web= new HtmlWeb();
            var doc=web.Load("https://www.xoom.com/sendmoneynow/india");
            var innerText = doc.DocumentNode.SelectSingleNode("//em[@class='fx-rate']").InnerText;
            var ratefromWeb=innerText.Split('=')[1].Split(' ')[1];
            var finalRate = (decimal) 0.0;
            if (decimal.TryParse(ratefromWeb, out finalRate))
            {
                return new ReturnedData()
                           {
                               ProviderName = "Xoom",
                               Rate = finalRate,
                               Fee = calculateFee(amount)
                           };
            }
            return null;

        }

        private decimal calculateFee(decimal amount)
        {
            if (amount <= 500)
                return (decimal) 2.99;
            if (amount > 500 && amount<=1000)
                return (decimal)4.99;
            if (amount > 1000)
                return (decimal) 0.00;

            return (decimal)0.00;
        }
    }
}