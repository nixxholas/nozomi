using System;
using System.Collections.Generic;
using Nozomi.Data.AreaModels.v1.CurrencyPair;
using Nozomi.Data.Models.Currency;

namespace Nozomi.Service.Services.Interfaces
{
    public interface ICurrencyPairService
    {
        bool Create(CreateCurrencyPair createCurrencyPair, long userId);
        long[][] GetCurrencySourceMappings();
        //IEnumerable<CurrencyPair> GetAvailCPairsForAdvType(long id);
        IEnumerable<CurrencyPair> GetAllActive(int index = 0, bool includeNested = false);
        IEnumerable<string> GetAllCurrencyPairUrls();
    }
}
