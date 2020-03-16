using Swashbuckle.AspNetCore.Filters;

namespace Nozomi.Api.Controllers.Request.Examples
{
    public class GetByGuidBadRequestExample : IExamplesProvider<string>
    {
        public const string Result = "Invalid Guid.";
        
        public string GetExamples()
        {
            return Result;
        }
    }
}