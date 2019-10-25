using System;

namespace Nozomi.Base.Core.Helpers
{
    public static class URLHelper
    {
        // https://stackoverflow.com/questions/7578857/how-to-check-whether-a-string-is-a-valid-http-url
        public static bool IsUrl(string uriName)
        {
            return Uri.TryCreate(uriName, UriKind.Absolute, out var uriResult)
                   && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }
    }
}