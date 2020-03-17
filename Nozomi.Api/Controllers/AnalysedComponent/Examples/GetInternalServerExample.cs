using Swashbuckle.AspNetCore.Filters;

namespace Nozomi.Api.Controllers.AnalysedComponent.Examples
{
    public class GetInternalServerExample : IExamplesProvider<string>
    {
        public const string Result = "Invalid API key.";
        
        public string GetExamples()
        {
            return Result;
        }
    }
}