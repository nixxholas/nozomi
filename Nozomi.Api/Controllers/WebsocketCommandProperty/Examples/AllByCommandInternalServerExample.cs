using Swashbuckle.AspNetCore.Filters;

namespace Nozomi.Api.Controllers.WebsocketCommandProperty.Examples
{
    public class AllByCommandInternalServerExample : IExamplesProvider<string>
    {
        public const string InvalidUserResult = "Please login again.";
        public const string ImpossibleInvalidUserResult = "You must be a genie. How did you do it?!";
        
        public string GetExamples()
        {
            return InvalidUserResult;
        }
    }
}