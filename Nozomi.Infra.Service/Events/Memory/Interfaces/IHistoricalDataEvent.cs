using System.Collections.Generic;
using Nozomi.Data.ResponseModels;

namespace Nozomi.Service.Events.Memory.Interfaces
{
    public interface IHistoricalDataEvent
    {
        ICollection<DistinctiveCurrencyResponse> GetSimpleCurrencyHistory(long sourceId, long days = 7);
    }
}