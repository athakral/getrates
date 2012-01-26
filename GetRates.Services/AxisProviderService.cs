using HtmlAgilityPack;
using System.Xml.Linq;
using System.Linq;

namespace GetRates.Services
{
    public class AxisProviderService : IProviderService
    {
        public ReturnedData GetRate(decimal amount)
        {
            var web = new HtmlWeb();
            var doc = web.Load("https://www.timesofmoney.com/remittance/axisremit/secure/axisremitExchangeRate.jsp?partnerId=AXISREMIT&uiId=AXISREMIT&defaultMenu=exchangeRate");
            var innerText = doc.DocumentNode.SelectSingleNode("//table[@class='itemGrid']").OuterHtml;
            XElement xElement = XElement.Parse(innerText);
            var chooseNext = false;
            var ratefromWeb = "";

            foreach (var x in xElement.Descendants("td").Skip(2))
            {
                if (chooseNext)
                {
                    ratefromWeb = x.Value;
                    break;
                }
                if (x.Value.Contains("-"))
                {
                    var minVal = x.Value.Split('-')[0];
                    var maxVal = x.Value.Split('-')[1];
                    try
                    {
                        if (amount >= decimal.Parse(minVal) && amount <= decimal.Parse(maxVal))
                            chooseNext = true;
                    }
                    catch (System.Exception)
                    {

                        break;
                    }

                }

            }
            
                    

               
            if (string.IsNullOrEmpty(ratefromWeb))
                return null;
            var finalRate = (decimal)0.0;
            if (decimal.TryParse(ratefromWeb, out finalRate))
            {
                return new ReturnedData()
                {
                    ProviderName = "<a href=\"https://www.timesofmoney.com/remittance/axisremit/secure/axisremitLoginForm.jsp?targeturl=https%3A%2F%2Fwww.timesofmoney.com%2Fremittance%2Faxisremit%2Fsecure%2Faxisremit_index.jsp\" target=\"_blank\">Axis</a>",
                    Rate = finalRate,
                    Deductions = calculateDeductions(finalRate, amount)
                };
            }
            return null;

        }

        private decimal calculateDeductions(decimal finalRate, decimal amount)
        {

            var convertedAmount = (finalRate * amount);

            var serviceTax = (decimal)25.75;

            if (convertedAmount <= 100000)
                if ((convertedAmount * (decimal)0.00103) > (decimal)25.75)
                    serviceTax = (convertedAmount * (decimal)0.00103);
            if (convertedAmount > 100000 && convertedAmount <= 1000000)
            {
                serviceTax = 103 + (convertedAmount * (decimal)0.00052);
            }
            if (serviceTax > 5150)
                serviceTax = 5150;

            return serviceTax;
        }

    }
}