using System;
using NUnit.Framework;
using RateCheck.Services;

namespace RateCheck.Tests
{
    [TestFixture]
    public class XoomProviderServiceTests
    {
        [Test]
        public void Can_GetRate()
        {
            var providerService = new XoomProviderService();
            var returnedValue=providerService.GetRate(100);
            Assert.Greater(returnedValue, 100);
            Console.WriteLine(returnedValue);
        }
         
    }
}