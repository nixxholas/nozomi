using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nozomi.Base.Core.Helpers.UI;
using Nozomi.Data;
using Nozomi.Data.ResponseModels;
using Nozomi.Data.ResponseModels.Ticker;

namespace Nozomi.Ticker.Areas.v1.Ticker
{
    public interface ITickerController
    {
        NozomiResult<string> Delete(string tickerSymbol, string exchangeAbbreviation);
        
        //Task<DataTableResult<UniqueTickerResponse>> GetAllForDataTables(int Draw = 0);
        
        Task<NozomiResult<ICollection<UniqueTickerResponse>>> GetAllAsync(int index = 0);

        NozomiResult<ICollection<TickerByExchangeResponse>> Get(string symbol, 
            string exchangeAbbrv = null);
    }
}