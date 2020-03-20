using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Nozomi.Auth.Controllers.ApiKey
{
    public interface IApiKeyController
    {
        Task<IActionResult> All();

        Task<IActionResult> Create(string label = null);

        Task<IActionResult> Revoke(string apiKey);
    }
}