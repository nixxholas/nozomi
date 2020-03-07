using Microsoft.AspNetCore.Mvc;
using Nozomi.Data.ViewModels.Request;

namespace Nozomi.Api.Controllers.Request
{
    public interface IRequestController
    {
        IActionResult Get(string guid);
    }
}