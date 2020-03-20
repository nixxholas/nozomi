using Microsoft.Extensions.Logging;
using Nozomi.Preprocessing.Abstracts;

namespace Nozomi.Api.Controllers.RequestProperties
{
    public class RequestPropertyController : BaseApiController<RequestPropertyController>, IRequestPropertyController
    {
        public RequestPropertyController(ILogger<RequestPropertyController> logger) : base(logger)
        {
        }
    }
}