using System;
using System.Text.RegularExpressions;

namespace Nozomi.Base.BCL.Helpers.Native.Text
{
    public static class StringHelper
    {
        public static bool IsAlphabeticalOnly(string text)
        {
            return Regex.IsMatch(text, @"^[a-zA-Z]+$");;
        }

        /// <summary>
        /// IsUrl method
        ///
        /// Enables the user to validate the string for URL conversion
        /// https://stackoverflow.com/questions/7578857/how-to-check-whether-a-string-is-a-valid-http-url#7581824
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static bool IsUrl(string url)
        {
            return Uri.TryCreate(url, UriKind.Absolute, out var uriResult) 
                          && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }
        
        public static bool IsValidEmail(string email)
        {
            try {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch {
                return false;
            }
        }
    }
}