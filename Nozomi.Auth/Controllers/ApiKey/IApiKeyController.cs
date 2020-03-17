using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Nozomi.Auth.Controllers.ApiKey
{
    public interface IApiKeyController
    {
        Task<IActionResult> Create();

        Task<IActionResult> Revoke(string apiKey);
    }
}