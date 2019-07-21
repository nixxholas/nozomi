using System.Collections.Generic;
using System.Threading.Tasks;
using Nozomi.Data;

namespace Nozomi.Web.Controllers.APIs.v1.AnalysedHistoricItem
{
    public interface IAnalysedHistoricItemController
    {
        Task<long> Count(long analysedComponentId);
        
        Task<NozomiResult<ICollection<Data.Models.Web.Analytical.AnalysedHistoricItem>>> GetAll(long analysedComponentId, int index = 0);
    }
}