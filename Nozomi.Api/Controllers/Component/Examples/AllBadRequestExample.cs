using Swashbuckle.AspNetCore.Filters;

namespace Nozomi.Api.Controllers.Component.Examples
{
    public class AllBadRequestExample : IExamplesProvider<string>
    {
        public const string Result = "Invalid index.";
        
        public string GetExamples()
        {
            return Result;
        }
    }
}