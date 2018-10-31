using System;
using System.Collections.Generic;
using Nozomi.Data.AreaModels.v1.CurrencyPair;
using Nozomi.Data.CurrencyModels;

namespace Nozomi.Service.Services.Interfaces
{
    public interface ICurrencyPairService
    {
        bool Create(CreateCurrencyPair createCurrencyPair, long userId);
        IEnumerable<dynamic> GetAllObsc(bool track = false);
        dynamic GetByIdObsc(long id, bool track = false);
        long GetCPairIdByTrio(long walletTypeId, long currencyId, long currencySourceId);
        ICollection<dynamic> GetAvailCPairsObsc(bool track = false);
        long[][] GetCurrencySourceMappings();
        //IEnumerable<CurrencyPair> GetAvailCPairsForAdvType(long id);
        IEnumerable<CurrencyPair> GetAllActive();
        IDictionary<string, IDictionary<long, long>> GetCurrencyPairSources();
        IEnumerable<string> GetAllCurrencyPairUrls();
    }
}
