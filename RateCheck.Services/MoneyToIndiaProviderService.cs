using System.IO;
using System.Net;
using HtmlAgilityPack;

namespace RateCheck.Services
{
    public class MoneyToIndiaProviderService : IProviderService
    {
        public ReturnedData GetRate(decimal amount) {
            var getCookieRequest =
                WebRequest.Create("https://m2inet.icicibank.co.in/m2iNet/exchangeRate.misc") as HttpWebRequest;
            if (getCookieRequest == null)
                return null;
            getCookieRequest.Method = "GET";
            getCookieRequest.UserAgent = " Mozilla/5.0 (Windows NT 6.1; WOW64; rv:9.0.1) Gecko/20100101 Firefox/9.0.1";
            getCookieRequest.ContentLength = 0;
            getCookieRequest.Accept = "text/plain, */*; q=0.01";
            getCookieRequest.KeepAlive = true;
            // getCookieRequest.Referer = " https://m2inet.icicibank.co.in/m2iNet/exchangeRate.misc";
            //getCookieRequest.CookieContainer = new CookieContainer();
            //getCookieRequest.CookieContainer.Add(new Cookie("JSESSIONID", "default-1287223533071854968", "/", "icicibank.co.in"));
            var getCookieResponse = getCookieRequest.GetResponse() as HttpWebResponse;
            if (getCookieResponse == null)
                return null;
            var stringValue = getCookieResponse.Headers.Get("Set-Cookie");

            //var reader = new StreamReader(webResponse.GetResponseStream());
            //var valueStr = reader.ReadToEnd();
            //reader.Close();
            //webResponse.Close();

            var request = WebRequest.Create(string.Format("https://m2inet.icicibank.co.in/m2iNet/exRateCalculator?productId=100002&&txnAmount={0}&&txnFixedAmount=0&&fixedInr=N&&deliveryMode=200001&&currency=99", amount)) as HttpWebRequest;
            if (request == null)
                return null;
            request.Method = "POST";
            request.UserAgent = " Mozilla/5.0 (Windows NT 6.1; WOW64; rv:9.0.1) Gecko/20100101 Firefox/9.0.1";
            request.ContentLength = 0;
            request.Accept = "text/plain, */*; q=0.01";
            request.KeepAlive = true;
            request.Referer = " https://m2inet.icicibank.co.in/m2iNet/exchangeRate.misc";
            request.CookieContainer = new CookieContainer();
            request.CookieContainer.Add(new Cookie(stringValue.Split(';')[0].Split('=')[0], stringValue.Split(';')[0].Split('=')[1], "/m2iNet", "icicibank.co.in"));
            var webResponse = request.GetResponse();
            var reader = new StreamReader(webResponse.GetResponseStream());
            var valueStr = reader.ReadToEnd();
            reader.Close();
            webResponse.Close();
            var valueArray = valueStr.Split('#');
            var serviceFeeWeb = valueArray[0];
            var finalAmount = valueArray[2];
            var ratefromWeb = valueArray[8];

            var finalValue = (decimal)0.0;
            var finalRate = (decimal)0.0;
            var serviceFee = (decimal)0.0;
            if (decimal.TryParse(finalAmount, out finalValue)
                && decimal.TryParse(ratefromWeb, out finalRate)
                && decimal.TryParse(serviceFeeWeb, out serviceFee)) {
                return new ReturnedData() {
                    ProviderName = "ICICI Bank Money To India",
                    Rate = finalRate,
                    Fee = serviceFee,
                    Deductions = (finalRate * amount) - finalValue
                };
            }
            return null;
        }
    }
}