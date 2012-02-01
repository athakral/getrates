using System.Collections.Generic;
using GetRates.Domain.Entities;

namespace GetRates.Services
{
    public interface IProviderService
    {
        IEnumerable<Provider> GetAll(decimal amount);
    }
}