using HtmlAgilityPack;
using System.Xml.Linq;
using System.Linq;

namespace GetRates.Services
{
    public class BoBProviderService : IProviderService
    {
        public ReturnedData GetRate(decimal amount)
        {
            var web = new HtmlWeb();
            var doc = web.Load("http://www.timesofmoney.com/remittance/jsp/remitExchangeRate.jsp?partnerId=BOB&uiId=BOB");

            var ratefromWeb = doc.DocumentNode.SelectNodes("//td").Skip(8).FirstOrDefault().InnerHtml;
            //XElement xElement = XElement.Parse(innerText);
            //var ratefromWeb = xElement.Descendants("td").Skip(7).FirstOrDefault().Value;

                       
            var finalRate = (decimal)0.0;
            if (decimal.TryParse(ratefromWeb, out finalRate))
            {
                return new ReturnedData()
                {
                    ProviderName = "<a href=\"http://www.bankofbaroda.com/int/nri.asp\" target=\"_blank\">Bank of Baroda</a>",
                    Rate = finalRate,
                    //Deductions = calculateDeductions(finalRate, amount)
                };
            }
            return null;

        }
    }
}