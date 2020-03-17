using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Nozomi.Auth.Controllers.ApiKey
{
    [SecurityHeaders]
    public class ApiKeyController : BaseController<ApiKeyController>, IApiKeyController
    {
        public ApiKeyController(ILogger<ApiKeyController> logger, IWebHostEnvironment webHostEnvironment) 
            : base(logger, webHostEnvironment)
        {
        }

        public IActionResult Create()
        {
            throw new System.NotImplementedException();
        }

        public IActionResult Revoke(string apiKey)
        {
            throw new System.NotImplementedException();
        }
    }
}