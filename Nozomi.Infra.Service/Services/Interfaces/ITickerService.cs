using System.Collections.Generic;
using System.Threading.Tasks;
using Nozomi.Data;
using Nozomi.Data.ResponseModels;

namespace Nozomi.Service.Services.Interfaces
{
    /// <summary>
    /// A publicly forward-facing service interface.
    /// </summary>
    public interface ITickerService
    {
        Task<NozomiResult<ICollection<UniqueTickerResponse>>> GetAll(int index);
        
        Task<NozomiResult<DistinctiveTickerResponse>> GetById(long id);

        NozomiResult<ICollection<DistinctiveTickerResponse>> GetByAbbreviation(string ticker, string exchangeAbbrv = null);

        NozomiResult<ICollection<DistinctiveTickerResponse>> GetAllActive();
    }
}