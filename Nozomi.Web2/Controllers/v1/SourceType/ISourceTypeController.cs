using Microsoft.AspNetCore.Mvc;
using Nozomi.Data.ViewModels.SourceType;

namespace Nozomi.Web2.Controllers.v1.SourceType
{
    public interface ISourceTypeController
    {
        IActionResult All();

        IActionResult Create(CreateSourceTypeViewModel vm);

        IActionResult Update(UpdateSourceTypeViewModel vm);
    }
}
