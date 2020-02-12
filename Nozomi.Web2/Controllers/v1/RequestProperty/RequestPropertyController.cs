using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nozomi.Data.ViewModels.RequestProperty;

namespace Nozomi.Web2.Controllers.v1.RequestProperty
{
    public class RequestPropertyController : BaseApiController<RequestPropertyController>, IRequestPropertyController
    {
        public RequestPropertyController(ILogger<RequestPropertyController> logger) : base(logger)
        {
        }

        public IActionResult Get(string guid)
        {
            throw new System.NotImplementedException();
        }

        public IActionResult GetAllByRequest(string requestGuid)
        {
            throw new System.NotImplementedException();
        }

        public IActionResult Create(CreateRequestPropertyInputModel vm)
        {
            throw new System.NotImplementedException();
        }

        public IActionResult Update(UpdateRequestPropertyInputModel vm)
        {
            throw new System.NotImplementedException();
        }

        public IActionResult Delete(string guid)
        {
            throw new System.NotImplementedException();
        }
    }
}