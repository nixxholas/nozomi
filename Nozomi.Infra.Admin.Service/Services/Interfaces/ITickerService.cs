using System.Collections.Generic;
using Nozomi.Base.Admin.Domain.AreaModels.Tickers;
using Nozomi.Data;
using Nozomi.Data.ResponseModels.Ticker;

namespace Nozomi.Infra.Admin.Service.Services.Interfaces
{
    /// <summary>
    /// A publicly forward-facing service interface.
    /// </summary>
    public interface ITickerService
    {
        /// <summary>
        /// Provides the functionality to process, prepare and create the ticker
        /// via the CTIM (CreateTickerInputModel) view model.
        /// </summary>
        /// <param name="createTickerInputModel">Data from the view.</param>
        /// <returns></returns>
        NozomiResult<UniqueTickerResponse> Create(CreateTickerInputModel createTickerInputModel);

        NozomiResult<string> Delete(string ticker, string exchangeAbbrv);

        NozomiResult<ICollection<TickerByExchangeResponse>> GetAllActive();
    }
}