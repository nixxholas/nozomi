using Microsoft.AspNetCore.Mvc;

namespace Nozomi.Auth.Controllers.ApiKey
{
    public interface IApiKeyController
    {
        IActionResult Create();

        IActionResult Revoke(string apiKey);
    }
}