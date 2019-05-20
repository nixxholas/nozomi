using System;
using System.Collections.Generic;
using Nozomi.Data.AreaModels.v1.CurrencyPair;
using Nozomi.Data.Models.Currency;

namespace Nozomi.Service.Services.Interfaces
{
    public interface ICurrencyPairService
    {
        bool Create(CreateCurrencyPair createCurrencyPair, long userId);
        IEnumerable<dynamic> GetAllObsc(bool track = false);
        long GetCPairIdByTrio(long walletTypeId, long currencyId, long currencySourceId);
        ICollection<dynamic> GetAvailCPairsObsc(int index = 0, bool track = false);
        long[][] GetCurrencySourceMappings();
        //IEnumerable<CurrencyPair> GetAvailCPairsForAdvType(long id);
        IEnumerable<CurrencyPair> GetAllActive(int index = 0, bool includeNested = false);
        IDictionary<string, IDictionary<long, long>> GetCurrencyPairSources();
        IEnumerable<string> GetAllCurrencyPairUrls();
    }
}
