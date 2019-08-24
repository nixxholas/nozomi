namespace Nozomi.Base.Core.Helpers.Exponent
{
    public class ExponentHelper
    {
        public static bool IsExponentialFormat(string str)
        {
            return (str.Contains("E") || str.Contains("e")) && double.TryParse(str, out _);
        }
    }
}