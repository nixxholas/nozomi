using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nozomi.Data;
using Nozomi.Data.ResponseModels;

namespace Nozomi.Ticker.Areas.v1.Ticker
{
    public interface ITickerController
    {
        Task<NozomiResult<ICollection<TickerResponse>>> GetAllAsync(int index = 0);

        NozomiResult<ICollection<DistinctiveTickerResponse>> Get(string symbol, bool includeNested = false);
    }
}