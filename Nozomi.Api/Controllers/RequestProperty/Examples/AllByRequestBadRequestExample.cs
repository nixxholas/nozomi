using Swashbuckle.AspNetCore.Filters;

namespace Nozomi.Api.Controllers.RequestProperty.Examples
{
    public class AllByRequestBadRequestExample : IExamplesProvider<string>
    {
        public const string InvalidIndexResult = "Invalid index.";

        public string GetExamples()
        {
            return InvalidIndexResult;
        }
    }
}