using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nozomi.Data;

namespace Nozomi.Web2.Controllers.v1.AnalysedHistoricItem
{
    public interface IAnalysedHistoricItemController
    {
        Task<long> Count(long analysedComponentId);
        
        Task<NozomiResult<ICollection<Data.Models.Web.Analytical.AnalysedHistoricItem>>> GetAll(long analysedComponentId, int index = 0);

        IActionResult List(string guid, int page = 0, int itemsPerPage = 0);
    }
}