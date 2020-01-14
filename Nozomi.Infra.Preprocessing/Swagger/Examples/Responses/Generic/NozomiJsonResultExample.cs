using Microsoft.AspNetCore.Mvc;
using Nozomi.Data;
using Swashbuckle.AspNetCore.Filters;

namespace Nozomi.Preprocessing.Swagger.Examples.Responses.Generic
{
    public class NozomiJsonResultExample : IExamplesProvider<NozomiResult<JsonResult>>
    {
        public NozomiResult<JsonResult> GetExamples()
        {
            return new NozomiResult<JsonResult>()
            {
                ResultType = NozomiResultType.Success,
                Message = "Your request was successfully fulfilled.",
                Data = new JsonResult("<payload>")
            };
        }
    }
}