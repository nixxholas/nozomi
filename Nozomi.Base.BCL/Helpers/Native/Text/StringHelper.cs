using System.Text.RegularExpressions;

namespace Nozomi.Base.BCL.Helpers.Native.Text
{
    public static class StringHelper
    {
        public static bool IsAlphabeticalOnly(string text)
        {
            return Regex.IsMatch(text, @"^[a-zA-Z]+$");;
        }
    }
}