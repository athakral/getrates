using System;
using System.Collections.Generic;
using System.Linq;
using GetRates.Domain.Entities;
using GetRates.Domain.Repositories;

namespace GetRates.Services
{
    public class ProviderService : IProviderService
    {
        private readonly IProviderRepository providerRepository;
        private readonly ILastCalledRepository lastCalledRepository;
        private readonly IEnumerable<IExchangeRateProvider> exchangeRateProviders;

        public ProviderService(IProviderRepository providerRepository,ILastCalledRepository lastCalledRepository,IEnumerable<IExchangeRateProvider> exchangeRateProviders)
        {
            this.providerRepository = providerRepository;
            this.lastCalledRepository = lastCalledRepository;
            this.exchangeRateProviders = exchangeRateProviders;
        }

        public IEnumerable<Provider> GetAll(decimal amount)
        {
            var currentDateTime = DateTime.Now;
            var lastCalledOn = lastCalledRepository.All.Max(k => k.CreatedOn);
            if (lastCalledOn.Subtract(currentDateTime).Hours > 2)
            {
                foreach (var exchangeRateProvider in exchangeRateProviders)
                {
                    var returnedData = exchangeRateProvider.GetRate(amount);
                }
                lastCalledRepository.InsertOrUpdate(new LastCalled());
            }
            return providerRepository.All.Where(k => k.CreatedOn > currentDateTime);
        }
    }
}