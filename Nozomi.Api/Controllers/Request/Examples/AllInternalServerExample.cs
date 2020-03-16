using Swashbuckle.AspNetCore.Filters;

namespace Nozomi.Api.Controllers.Request.Examples
{
    public class AllInternalServerExample : IExamplesProvider<string>
    {
        public const string Result = "Not sure how you got here, but no.";
        
        public string GetExamples()
        {
            return Result;
        }
    }
}