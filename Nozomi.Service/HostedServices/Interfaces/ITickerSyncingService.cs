using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Nozomi.Data.CurrencyModels;

namespace Nozomi.Service.HostedServices.Interfaces
{
    public interface ITickerSyncingService
    {
        /// <summary>
        /// Polls the data from externals.
        /// 
        /// Iterate from every service, returns the result in string.
        /// </summary>
        /// <returns>Returns the id of the pair and the result in decimal.</returns>
        Task<Dictionary<long, decimal>> PollDataFromExternalsAsync(IEnumerable<CurrencyPair> cPairs);
    }
}
