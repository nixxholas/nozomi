using Swashbuckle.AspNetCore.Filters;

namespace Nozomi.Api.Controllers.Component.Examples
{
    public class GetBadRequestExample : IExamplesProvider<string>
    {
        public const string Result = "Invalid guid or index.";
        
        public string GetExamples()
        {
            return Result;
        }
    }
}