using System;
using System.Threading.Tasks;
using Nozomi.Data.Models.Currency;

namespace Nozomi.Service.HostedServices.Interfaces
{
    public interface ICurrencyPairCacheManagementService
    {
        /// <summary>
        /// Bootstrap the Currency Pair table to the Memory Cache.
        /// </summary>
        void InitializeCache(IServiceProvider serviceProvider);
        
        /// <summary>
        /// Sends the currency pair for in-processing,
        ///
        /// To validate and verify its existence in accordance to the database.
        /// </summary>
        /// <param name="currencyPair"></param>
        /// <returns></returns>
        Task InproPair(CurrencyPair currencyPair);
    }
}