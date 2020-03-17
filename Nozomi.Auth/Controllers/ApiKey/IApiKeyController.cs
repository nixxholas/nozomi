using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Nozomi.Auth.Controllers.ApiKey
{
    public interface IApiKeyController
    {
        Task<IActionResult> All();

        Task<IActionResult> Get(string apiKey);
        
        Task<IActionResult> Create();

        Task<IActionResult> Revoke(string apiKey);
    }
}