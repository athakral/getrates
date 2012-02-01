using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using GetRates.Domain.Base;
using GetRates.Services;

namespace GetRates.Tests
{
    [TestFixture]
    public class BaseRepositoryTests
    {
        [Test]
        public void CanStoreInMongoHQ()
        {
            var repo = new BaseRepositoryMongo<TestData, Guid>("testDoc");
            var id = Guid.NewGuid();
            var actualObj=new TestData() { Name = "asfdasdfa", Id=id };
            repo.InsertOrUpdate(actualObj);
            var returnedObj = repo.Find(id);
            Assert.AreEqual(actualObj.Id,returnedObj.Id);
            Assert.AreEqual(actualObj.Name, returnedObj.Name);
        }


    }

    public class TestData
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
    }
}
