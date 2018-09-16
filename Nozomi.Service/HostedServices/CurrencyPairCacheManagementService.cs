using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nozomi.Data.CurrencyModels;
using Nozomi.Service.HostedServices.Interfaces;
using Nozomi.Service.Services;
using Nozomi.Service.Services.Interfaces;
using StackExchange.Redis;

namespace Nozomi.Service.HostedServices
{
    public class CurrencyPairCacheManagementService : BaseHostedService, ICurrencyPairCacheManagementService
    {
        private ICurrencyPairService _currencyPairService;
        private IConnectionMultiplexer _connectionMultiplexer;
        private ILogger<CurrencyPairCacheManagementService> _logger;
        
        public CurrencyPairCacheManagementService(IConnectionMultiplexer connectionMultiplexer,
            IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _currencyPairService = serviceProvider.GetService<CurrencyPairService>();
            _connectionMultiplexer = connectionMultiplexer;
            _logger = _scope.ServiceProvider.GetRequiredService<ILogger<CurrencyPairCacheManagementService>>();
            
            InitializeCache(serviceProvider);
        }

        /// <summary>
        /// Operation Procedure for CurrencyPair Cache Management.
        ///
        /// Updates every 5 seconds.
        ///
        /// Objectives:
        /// 1. Pull the latest currency pair dataset from cold storage (DB)
        /// 2. Cross reference checking (MemoryCache vs Cold Storage)
        /// 3. Update Currency pairs
        /// </summary>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("CurrencyPairCacheManagementService is starting.");

            stoppingToken.Register(() => _logger.LogInformation("CurrencyPairCacheManagementService is stopping."));

            while (!stoppingToken.IsCancellationRequested)
            {
                var currencyPairs = _currencyPairService.GetAllActive();
                
                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }

            _logger.LogWarning("CurrencyPairCacheManagementService background task is stopping.");
        }

        public void InitializeCache(IServiceProvider serviceProvider)
        {
            var currencyPairs = _currencyPairService.GetAllActive();

            // Load them individually to the cache.
            // This way, we won't have to update the entire collection if we were to remove, update or add one.
            foreach (var cPair in currencyPairs)
            {
                // Naming convention => PREFIX + CURRENCYPAIRID
                // Set the object into the cache
                
            }
        }

        public Task InproPair(CurrencyPair currencyPair)
        {
            throw new NotImplementedException();
        }
    }
}