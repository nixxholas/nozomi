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
        Task<NozomiResult<TickerResponse>> GetById(long id);

        NozomiResult<ICollection<TickerResponse>> GetByAbbreviation(string ticker);
    }
}