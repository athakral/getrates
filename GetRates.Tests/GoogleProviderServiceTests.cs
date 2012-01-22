using System;
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
            var providerService = new GoogleProviderService();
            var returnedValue=providerService.GetRate(100);
            Assert.NotNull(returnedValue);
            Assert.Greater(returnedValue.Rate, 35);
            Console.WriteLine(returnedValue.Rate);
        }
         
    }
}