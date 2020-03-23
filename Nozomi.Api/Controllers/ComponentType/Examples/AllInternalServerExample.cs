namespace Nozomi.Api.Controllers.ComponentType.Examples
{
    public class AllInternalServerExample
    {
        public const string Result = "You might want to beep us via api@nozomi.one to follow up with this issue!";
        
        public string GetExamples()
        {
            return Result;
        }
    }
}