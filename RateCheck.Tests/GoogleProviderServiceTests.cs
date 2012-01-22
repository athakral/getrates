﻿using System;
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
            Assert.NotNull(returnedValue);
            Assert.Greater(returnedValue.Rate, 35);
            Console.WriteLine(returnedValue.Rate);
        }
         
    }
}