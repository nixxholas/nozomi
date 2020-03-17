using Swashbuckle.AspNetCore.Filters;

namespace Nozomi.Api.Controllers.Connect.Examples
{        
    public class ValidateOkExample : IExamplesProvider<string>
    {
        public const string Result = "Hey you're legit!";
            
        public string GetExamples()
        {
            return Result;
        }
    }
}