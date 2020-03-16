using Swashbuckle.AspNetCore.Filters;

namespace Nozomi.Api.Controllers.Connect.Examples
{
    public class ValidateBadRequestExample : IExamplesProvider<string>
    {
        public const string Result = "Invalid API Key.";
            
        public string GetExamples()
        {
            return Result;
        }
    }
}