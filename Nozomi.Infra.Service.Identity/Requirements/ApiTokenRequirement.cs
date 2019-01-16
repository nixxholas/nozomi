using Microsoft.AspNetCore.Authorization;

namespace Nozomi.Service.Identity.Requirements
{
    public class ApiTokenRequirement : IAuthorizationRequirement
    {
        public const string ApiTokenRequirementName = "ApiTokenRequired";
        
        public ApiTokenRequirement()
        {
        }
    }
}