using System;
using GetRates.Domain.Base;
using GetRates.Domain.Entities;

namespace GetRates.Domain.Repositories
{
    public interface ILastCalledRepository : IBaseRepository<LastCalled, Guid>
    {
         
    }
}