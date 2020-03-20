using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nozomi.Preprocessing.Abstracts;

namespace Nozomi.Api.Controllers.ComponentHistoricItem
{
    public class ComponentHistoricItemController : BaseApiController<ComponentHistoricItemController>, 
        IComponentHistoricItemController
    {
        public ComponentHistoricItemController(ILogger<ComponentHistoricItemController> logger) : base(logger)
        {
        }

        public IActionResult All(int index = 0)
        {
            throw new System.NotImplementedException();
        }

        public IActionResult Get(string guid, int index = 0)
        {
            throw new System.NotImplementedException();
        }
    }
}