using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Nozomi.Preprocessing.ActionResults
{
    public class InternalServerErrorObjectResult : ObjectResult
    {
        public InternalServerErrorObjectResult(object error)
            : base(error)
        {
            StatusCode = StatusCodes.Status500InternalServerError;
        }
    }
}