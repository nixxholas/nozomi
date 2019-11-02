using Microsoft.AspNetCore.Mvc;
using Nozomi.Data.ResponseModels.SourceType;

namespace Nozomi.Web.Controllers.APIs.v1.SourceType
{
    public interface ISourceTypeController
    {
        IActionResult All();

        IActionResult Create(CreateSourceTypeViewModel vm);
    }
}
