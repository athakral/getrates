using System.IO;
using System.Net;
using System.Text;
using HtmlAgilityPack;

namespace RateCheck.Services
{
    public class RemitToIndiaProviderService : IProviderService
    {
        public float GetRate(float amount)
        {
            var request = WebRequest.Create("http://www.timesofmoney.com/remittance/jsp/r2iExchRateCalculator.jsp?strAction=show&partnerSite=TOML&sendercountry=US&sendercurrency=USD&uiId=TOML") as HttpWebRequest;
            if (request == null)
                return (float)0.0;
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
            var finalRate = (float)0.0;
            if (float.TryParse(ratefromWeb, out finalRate)) {
                return amount * finalRate;
            }
            return (float)0.0;

        }
    }
}