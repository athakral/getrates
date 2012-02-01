using System;
using GetRates.Services.Providers;
using NUnit.Framework;
using GetRates.Services;

namespace GetRates.Tests
{
    [TestFixture]
    public class GoogleProviderServiceTests
    {
        [Test]
        public void Can_GetRate()
        {
            var providerService = new GoogleExchangeRateProvider();
            var returnedValue=providerService.GetRate(100);
            Assert.NotNull(returnedValue);
            Assert.Greater(returnedValue.Rate, 35);
            Console.WriteLine(returnedValue.Rate);
        }
         
    }
}