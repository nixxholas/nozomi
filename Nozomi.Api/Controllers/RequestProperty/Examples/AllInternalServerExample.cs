using Swashbuckle.AspNetCore.Filters;

namespace Nozomi.Api.Controllers.RequestProperty.Examples
{
    public class AllInternalServerExample : IExamplesProvider<string>
    {
        public const string InvalidUserResult = "Please login again.";
        public const string ImpossibleInvalidUserResult = "You must be a genie. How did you do it?!";

        public string GetExamples()
        {
            return InvalidUserResult;
        }
    }
}