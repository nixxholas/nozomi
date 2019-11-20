using System.Collections.Generic;
using System.Threading.Tasks;
using Nozomi.Data;
using Nozomi.Data.ResponseModels.Ticker;
using Nozomi.Data.ResponseModels.TickerPair;

namespace Nozomi.Web2.Controllers.APIs.v1.Ticker
{
    public interface ITickerController
    {
        NozomiResult<ICollection<TickerPairResponse>> GetTickerPairSources();

        Task<NozomiResult<ICollection<UniqueTickerResponse>>> GetAllAsync(int index = 0);

        NozomiResult<ICollection<TickerByExchangeResponse>> Get(string symbol,
            string exchangeAbbrv = null);
    }
}
