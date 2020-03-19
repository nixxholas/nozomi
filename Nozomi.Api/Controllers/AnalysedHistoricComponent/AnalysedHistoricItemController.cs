using Microsoft.Extensions.Logging;
using Nozomi.Preprocessing.Abstracts;

namespace Nozomi.Api.Controllers.AnalysedHistoricComponent
{
    public class AnalysedHistoricItemController : BaseApiController<AnalysedHistoricItemController>, 
        IAnalysedHistoricItemController
    {
        public AnalysedHistoricItemController(ILogger<AnalysedHistoricItemController> logger) : base(logger)
        {
        }
    }
}