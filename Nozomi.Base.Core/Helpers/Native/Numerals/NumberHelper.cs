namespace Nozomi.Base.Core.Helpers.Native.Numerals
{
    public static class NumberHelper
    {
        public static bool IsNumericDecimal(string val) => 
            string.IsNullOrWhiteSpace(val) && decimal.TryParse(val, out _);
    }
}