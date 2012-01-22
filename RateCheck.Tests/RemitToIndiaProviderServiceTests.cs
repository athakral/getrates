using System;
using NUnit.Framework;
using RateCheck.Services;

namespace RateCheck.Tests
{
    [TestFixture]
    public class RemitToIndiaProviderServiceTests
    {
        [Test]
        public void Can_GetRate()
        {
            var providerService = new RemitToIndiaProviderService();
            var returnedValue=providerService.GetRate(100);
            Assert.NotNull(returnedValue);
            Assert.Greater(returnedValue.Rate, 100);
            Console.WriteLine(returnedValue.Rate);
        }
         
    }
}