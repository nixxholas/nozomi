using Microsoft.AspNetCore.Mvc;

namespace Nozomi.Api.Controllers.RequestProperties
{
    public interface IRequestPropertyController
    {
        IActionResult All(int index = 0);
        
        IActionResult AllByRequest(string requestGuid, int index = 0);
    }
}