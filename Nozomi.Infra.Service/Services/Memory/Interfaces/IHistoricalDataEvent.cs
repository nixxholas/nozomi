using System.Collections.Generic;
using Nozomi.Data.ResponseModels;

namespace Nozomi.Service.Services.Memory.Interfaces
{
    public interface IHistoricalDataEvent
    {
        ICollection<DistinctiveCurrencyResponse> GetSimpleCurrencyHistory(long sourceId, long days = 7);
    }
}