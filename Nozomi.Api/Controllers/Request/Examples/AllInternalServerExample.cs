using Swashbuckle.AspNetCore.Filters;

namespace Nozomi.Api.Controllers.Request.Examples
{
    public class AllInternalServerExample : IExamplesProvider<string>
    {
        public const string InvalidUserResult = "Please login again.";
        public const string Result = "You might want to beep us via api@nozomi.one to follow up with this issue!";
        
        public string GetExamples()
        {
            return Result;
        }
    }
}