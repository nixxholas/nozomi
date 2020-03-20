using Swashbuckle.AspNetCore.Filters;

namespace Nozomi.Api.Controllers.RequestProperties.Examples
{
    public class AllBadRequestExample : IExamplesProvider<string>
    {
        public const string InvalidIndexResult = "Invalid index.";

        public string GetExamples()
        {
            return InvalidIndexResult;
        }
    }
}