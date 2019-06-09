namespace Nozomi.Base.Core.Helpers.Native.Numerals
{
    public static class NumberHelper
    {
        public static bool IsNumericDecimal(string val) => decimal.TryParse(val, out var result);
    }
}