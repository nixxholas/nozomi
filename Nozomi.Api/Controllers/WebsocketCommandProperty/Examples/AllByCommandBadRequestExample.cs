using Swashbuckle.AspNetCore.Filters;

namespace Nozomi.Api.Controllers.WebsocketCommandProperty.Examples
{
    public class AllByRequestBadRequestExample : IExamplesProvider<string>
    {
        public const string Result = "Invalid index.";
        
        public string GetExamples()
        {
            return Result;
        }
    }
}