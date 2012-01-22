using System.IO;
using System.Net;
using System.Text;
using HtmlAgilityPack;

namespace RateCheck.Services
{
    public class RemitToIndiaProviderService : IProviderService
    {
        public ReturnedData GetRate(decimal amount) {
            var request = WebRequest.Create("http://www.timesofmoney.com/remittance/jsp/r2iExchRateCalculator.jsp?strAction=show&partnerSite=TOML&sendercountry=US&sendercurrency=USD&uiId=TOML") as HttpWebRequest;
            if (request == null)
                return null;
            request.Method = "POST";
            request.UserAgent = " Mozilla/5.0 (Windows NT 6.1; WOW64; rv:9.0.1) Gecko/20100101 Firefox/9.0.1";
            request.Accept = "text/plain, */*; q=0.01";
            request.KeepAlive = true;
            request.Referer = "http://www.timesofmoney.com/remittance/jsp/r2iExchRateCalculator.jsp?uiId=TOML&sendercountry=US&sendercurrency=USD";

            // Create POST data and convert it to a byte array.
            string postData = string.Format("action1=doCalculation&grates=&selCountry=US-USD&payMode=ACH&rate=NORMAL&rate1=NORMAL&payMode=WIRE&rmtAmount={0}", amount);
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            // Set the ContentType property of the WebRequest.
            request.ContentType = "application/x-www-form-urlencoded";
            // Set the ContentLength property of the WebRequest.
            request.ContentLength = byteArray.Length;
            // Get the request stream.
            Stream dataStream = request.GetRequestStream();
            // Write the data to the request stream.
            dataStream.Write(byteArray, 0, byteArray.Length);
            // Close the Stream object.
            dataStream.Close();

            // request.CookieContainer = new CookieContainer();
            //request.CookieContainer.Add(new Cookie(stringValue.Split(';')[0].Split('=')[0], stringValue.Split(';')[0].Split('=')[1], "/m2iNet", "icicibank.co.in"));
            var webResponse = request.GetResponse();
            var reader = new StreamReader(webResponse.GetResponseStream());
            var valueStr = reader.ReadToEnd();
            reader.Close();
            webResponse.Close();
            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(valueStr);
            var ratefromWeb = document.DocumentNode.SelectSingleNode("//span[@id='rateDisplay']").InnerText;
            var finalRate = (decimal)0.0;
            if (decimal.TryParse(ratefromWeb, out finalRate)) {
                return new ReturnedData() {
                    ProviderName = "Times Money Remit 2 India",
                    Rate = finalRate,
                    Fee = calculateFee(amount),
                    Deductions = calculateDeductions(finalRate, amount)

                };
            }
            return null;

        }

        private decimal calculateDeductions(decimal finalRate, decimal amount) {
            var tempDeduct = (decimal)0.00;
            if (amount <= 500)
                tempDeduct = 35;
            if (amount > 500 && amount <= 1000)
                tempDeduct = 45;
            if (amount > 1000 && amount <= 5000)
                tempDeduct = 75;
            if (amount > 5000)
                tempDeduct = 125;

            var convertedAmount = (finalRate * amount) - tempDeduct;

            //refer to tax table on http://www.timesofmoney.com/remittance/jsp/r2i_low_transfer_fees_money_transfer_asia.jsp?uiId=TOML&tab=US&sendercountry=US&sendercurrency=USD

            var serviceTax = (decimal)25.75;

            if (convertedAmount <= 100000)
                if ((convertedAmount * (decimal)0.00103) > (decimal)25.75)
                    serviceTax = (convertedAmount * (decimal)0.00103);
            if (convertedAmount > 100000 && convertedAmount <= 1000000) {
                serviceTax = 103 + (convertedAmount * (decimal)0.00052);
            }
            if(convertedAmount > 1000000)
                serviceTax = 567 + (convertedAmount * (decimal)0.000103);

            if (serviceTax > 5150)
                serviceTax = 5150;

            return tempDeduct + serviceTax;
        }

        private decimal calculateFee(decimal amount) {
            if (amount <= 500)
                return (decimal)3;
            if (amount > 500 && amount <= 1000)
                return (decimal)5;
            if (amount > 1000)
                return (decimal)0.00;
            return (decimal)0.00;
        }
    }
}