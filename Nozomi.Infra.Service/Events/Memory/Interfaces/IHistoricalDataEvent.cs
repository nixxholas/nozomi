using System.Collections.Generic;
using Nozomi.Data.ResponseModels;
using Nozomi.Data.ResponseModels.Currency;

namespace Nozomi.Service.Events.Memory.Interfaces
{
    public interface IHistoricalDataEvent
    {
        ICollection<DistinctiveCurrencyResponse> GetSimpleCurrencyHistory(long sourceId, long days = 7);
    }
}