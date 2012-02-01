using GetRates.Domain.Entities;
using GetRates.Domain.Repositories;
using NUnit.Framework;

namespace GetRates.Tests
{
    [TestFixture]
    public class RateRepositoryTests
    {
        [Test]
        public void Can_Save_Rates()
        {
            var rateRepo = new ProviderRepositoryMongo();
            var rateEntActual = new Provider();
            rateRepo.InsertOrUpdate(rateEntActual);
            var returnedRate = rateRepo.Find(rateEntActual.Id);
            Assert.AreEqual(returnedRate.Id,rateEntActual.Id);


        }
    }
}