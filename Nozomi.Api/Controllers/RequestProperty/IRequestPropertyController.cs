using Microsoft.AspNetCore.Mvc;

namespace Nozomi.Api.Controllers.RequestProperty
{
    public interface IRequestPropertyController
    {
        IActionResult All(int index = 0);
        
        IActionResult AllByRequest(string requestGuid, int index = 0);
    }
}