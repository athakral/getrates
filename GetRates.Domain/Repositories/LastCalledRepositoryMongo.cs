using System;
using GetRates.Domain.Base;
using GetRates.Domain.Entities;

namespace GetRates.Domain.Repositories
{
    public class LastCalledRepositoryMongo : BaseRepositoryMongo<LastCalled, Guid>, ILastCalledRepository
    {
        public LastCalledRepositoryMongo()
            : base("lastCalled")
        {
        }
    }
}