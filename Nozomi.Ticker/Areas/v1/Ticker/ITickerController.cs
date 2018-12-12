using System.Collections;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Nozomi.Data;
using Nozomi.Data.ResponseModels;

namespace Nozomi.Ticker.Areas.v1.Ticker
{
    public interface ITickerController
    {
        NozomiResult<ICollection<DistinctiveTickerResponse>> Get(string symbol, bool includeNested = false);
    }
}