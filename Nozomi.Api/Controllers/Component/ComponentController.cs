using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nozomi.Preprocessing.Abstracts;

namespace Nozomi.Api.Controllers.Component
{
    public class ComponentController : BaseApiController<ComponentController>, IComponentController
    {
        public ComponentController(ILogger<ComponentController> logger) : base(logger)
        {
        }

        public IActionResult All(int index = 0, string requestGuid = null)
        {
            throw new System.NotImplementedException();
        }

        public IActionResult Get(string guid, int index = 0)
        {
            throw new System.NotImplementedException();
        }
    }
}