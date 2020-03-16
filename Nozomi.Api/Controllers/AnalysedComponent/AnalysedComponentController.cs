using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nozomi.Preprocessing.Abstracts;

namespace Nozomi.Api.Controllers.AnalysedComponent
{
    public class AnalysedComponentController : BaseApiController<AnalysedComponentController>,
        IAnalysedComponentController
    {
        public AnalysedComponentController(ILogger<AnalysedComponentController> logger) : base(logger)
        {
        }

        [HttpGet]
        public IActionResult All()
        {
            throw new System.NotImplementedException();
        }

        [HttpGet]
        public IActionResult AllByIdentifier(string currencySlug, string currencyPairGuid, 
            string currencyTypeShortForm, int index = 0, int itemsPerPage = 200)
        {
            throw new System.NotImplementedException();
        }

        [HttpGet]
        public IActionResult Get(string guid)
        {
            throw new System.NotImplementedException();
        }
    }
}