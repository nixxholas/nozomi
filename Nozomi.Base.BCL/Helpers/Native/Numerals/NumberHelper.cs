using System;

namespace Nozomi.Base.BCL.Helpers.Native.Numerals
{
    public static class NumberHelper
    {
        public static bool IsNumericDecimal(string val) => 
            !string.IsNullOrWhiteSpace(val) && decimal.TryParse(val, out _);
        
        /// <summary>
        /// Returns an Int32 with a random value across the entire range of
        /// possible values.
        ///
        /// https://stackoverflow.com/questions/609501/generating-a-random-decimal-in-c-sharp
        /// </summary>
        public static int GenerateInt32(this Random rng)
        {
            int firstBits = rng.Next(0, 1 << 4) << 28;
            int lastBits = rng.Next(0, 1 << 28);
            return firstBits | lastBits;
        }

        public static decimal GenerateDecimal(this Random rng)
        {
            byte scale = (byte) rng.Next(29);
            bool sign = rng.Next(2) == 1;
            return new decimal(rng.GenerateInt32(), 
                rng.GenerateInt32(),
                rng.GenerateInt32(),
                sign,
                scale);
        }
    }
}