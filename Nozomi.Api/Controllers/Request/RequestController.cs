using Microsoft.Extensions.Logging;
using Nozomi.Preprocessing.Abstracts;

namespace Nozomi.Api.Controllers.Request
{
    public class RequestController : BaseApiController<RequestController>, IRequestController
    {
        public RequestController(ILogger<RequestController> logger) : base(logger)
        {
        }
    }
}