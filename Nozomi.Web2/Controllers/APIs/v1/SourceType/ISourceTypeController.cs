using Microsoft.AspNetCore.Mvc;
using Nozomi.Data.ResponseModels.SourceType;

namespace Nozomi.Web2.Controllers.APIs.v1.SourceType
{
    public interface ISourceTypeController
    {
        IActionResult All();

        IActionResult Create(CreateSourceTypeViewModel vm);
    }
}
