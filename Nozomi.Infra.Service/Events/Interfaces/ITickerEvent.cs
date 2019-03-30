using System.Collections.Generic;
using System.Threading.Tasks;
using Nozomi.Base.Core.Helpers.UI;
using Nozomi.Data;
using Nozomi.Data.ResponseModels;
using Nozomi.Data.ResponseModels.Ticker;
using Nozomi.Data.ResponseModels.TickerPair;

namespace Nozomi.Service.Events.Interfaces
{
    public interface ITickerEvent
    {
        DataTableResult<UniqueTickerResponse> GetAllForDatatable(int index = 0);
        
        Task<NozomiResult<TickerByExchangeResponse>> GetById(long id);

        ICollection<TickerPairResponse> GetAllTickerPairSources();

        NozomiResult<ICollection<TickerByExchangeResponse>> GetByAbbreviation(string ticker, string exchangeAbbrv = null);
    }
}