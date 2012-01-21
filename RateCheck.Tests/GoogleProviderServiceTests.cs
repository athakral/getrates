using System;
using NUnit.Framework;
using RateCheck.Services;

namespace RateCheck.Tests
{
    [TestFixture]
    public class GoogleProviderServiceTests
    {
        [Test]
        public void Can_GetRate()
        {
            var providerService = new GoogleProviderService();
            var returnedValue=providerService.GetRate(100);
            Assert.Greater(returnedValue, 100);
            Console.WriteLine(returnedValue);
        }
         
    }
}