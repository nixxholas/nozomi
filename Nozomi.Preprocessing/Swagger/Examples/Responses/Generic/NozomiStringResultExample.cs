using Nozomi.Data;
using Swashbuckle.AspNetCore.Filters;

namespace Nozomi.Preprocessing.Swagger.Examples.Responses.Generic
{
    public class NozomiStringResultExample : IExamplesProvider<NozomiResult<string>>
    {
        public NozomiResult<string> GetExamples()
        {
            return new NozomiResult<string>()
            {
                ResultType = NozomiResultType.Success,
                Message = "Your request was successfully fulfilled.",
                Data = "Nothing really, since 'Message' does the job already."
            };
        }
    }
}