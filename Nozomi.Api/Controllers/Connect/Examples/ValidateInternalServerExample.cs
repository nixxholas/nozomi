using Swashbuckle.AspNetCore.Filters;

namespace Nozomi.Api.Controllers.Connect.Examples
{
    public class ValidateInternalServerExample : IExamplesProvider<string>
    {
        public const string Result = "Where's your API Key mate?";

        public string GetExamples()
        {
            return Result;
        }
    }
}