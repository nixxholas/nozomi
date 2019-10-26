using System;

namespace Nozomi.Base.Core.Helpers
{
    public static class URLHelper
    {
        public static bool IsUrl(string uriName)
        {
            return Uri.TryCreate(uriName, UriKind.Absolute, out var uriResult)
                   && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }
    }
}