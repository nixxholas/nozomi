using Microsoft.AspNetCore.Authentication;

namespace Nozomi.Preprocessing.Options
{
    public class ApiKeyAuthenticationOptions : AuthenticationSchemeOptions
    {
        public const string DefaultScheme = "API Key";
        public string Scheme => DefaultScheme;
        public string AuthenticationType = DefaultScheme;
        public const string HeaderKey = "X-Api-Key";
    }
}