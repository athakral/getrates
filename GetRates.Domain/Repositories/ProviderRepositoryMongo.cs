using System;
using GetRates.Domain.Base;
using GetRates.Domain.Entities;

namespace GetRates.Domain.Repositories
{
    public class ProviderRepositoryMongo : BaseRepositoryMongo<Provider,Guid>,IProviderRepository
    {
        public ProviderRepositoryMongo() : base("rates")
        {
        }
    }
}