using System.IO;
using System.Net;
using HtmlAgilityPack;

namespace RateCheck.Services
{
    public class MoneyToIndiaProviderService : IProviderService
    {
        public float GetRate(float amount)
        {
            var getCookieRequest =
                WebRequest.Create("https://m2inet.icicibank.co.in/m2iNet/exchangeRate.misc") as HttpWebRequest;
            if (getCookieRequest == null)
                return (float)0.0;
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
                return (float)0.0;
            var stringValue=getCookieResponse.Headers.Get("Set-Cookie");
            
            //var reader = new StreamReader(webResponse.GetResponseStream());
            //var valueStr = reader.ReadToEnd();
            //reader.Close();
            //webResponse.Close();

            var request = WebRequest.Create(string.Format("https://m2inet.icicibank.co.in/m2iNet/exRateCalculator?productId=100002&&txnAmount={0}&&txnFixedAmount=0&&fixedInr=N&&deliveryMode=200001&&currency=99",amount)) as HttpWebRequest;
            if (request == null)
                return (float) 0.0;
            request.Method = "POST";
            request.UserAgent = " Mozilla/5.0 (Windows NT 6.1; WOW64; rv:9.0.1) Gecko/20100101 Firefox/9.0.1";
            request.ContentLength = 0;
            request.Accept="text/plain, */*; q=0.01";
            request.KeepAlive = true;
            request.Referer = " https://m2inet.icicibank.co.in/m2iNet/exchangeRate.misc";
            request.CookieContainer=new CookieContainer();
            request.CookieContainer.Add(new Cookie(stringValue.Split(';')[0].Split('=')[0], stringValue.Split(';')[0].Split('=')[1], "/m2iNet", "icicibank.co.in"));
            var webResponse=request.GetResponse();
            var reader=new StreamReader(webResponse.GetResponseStream());
            var valueStr = reader.ReadToEnd();
            reader.Close();
            webResponse.Close();
            var valueArray = valueStr.Split('#');
            var finalAmount = valueArray[2];
            //var finalRate = valueArray[4];
            //var web = new HtmlWeb();
            //var doc = web.Load("https://m2inet.icicibank.co.in/m2iNet/exRateCalculator?productId=100002&&txnAmount=2500&&txnFixedAmount=0&&fixedInr=N&&deliveryMode=200001&&currency=99", "POST");
            //var innerText = doc.DocumentNode.SelectSingleNode("//em[@class='fx-rate']").InnerText;
            //var ratefromWeb = innerText.Split('=')[1].Split(' ')[1];
            var finalValue = (float)0.0;
            if (float.TryParse(finalAmount, out finalValue)) {
                return finalValue;
            }
            return (float)0.0;
        }
    }
}