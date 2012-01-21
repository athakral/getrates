using HtmlAgilityPack;

namespace RateCheck.Services
{
    public class XoomProviderService : IProviderService
    {
        public float GetRate(float amount)
        {
            var web= new HtmlWeb();
            var doc=web.Load("https://www.xoom.com/sendmoneynow/india");
            var innerText = doc.DocumentNode.SelectSingleNode("//em[@class='fx-rate']").InnerText;
            var ratefromWeb=innerText.Split('=')[1].Split(' ')[1];
            var finalRate = (float) 0.0;
            if (float.TryParse(ratefromWeb, out finalRate))
            {
                return amount*finalRate;
            }
            return (float) 0.0;

        }
    }
}