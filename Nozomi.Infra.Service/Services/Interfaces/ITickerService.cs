using System.Collections.Generic;
using System.Threading.Tasks;
using Nozomi.Base.Identity.ViewModels.Manage.Tickers;
using Nozomi.Data;
using Nozomi.Data.ResponseModels;
using Nozomi.Data.ResponseModels.Ticker;

namespace Nozomi.Service.Services.Interfaces
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
        
        Task<NozomiResult<ICollection<UniqueTickerResponse>>> GetAll(int index);
        
        Task<NozomiResult<DistinctiveTickerResponse>> GetById(long id);

        NozomiResult<ICollection<DistinctiveTickerResponse>> GetByAbbreviation(string ticker, string exchangeAbbrv = null);

        NozomiResult<ICollection<DistinctiveTickerResponse>> GetAllActive();
    }
}