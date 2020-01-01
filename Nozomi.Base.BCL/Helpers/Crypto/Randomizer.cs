using System;
using System.Security.Cryptography;

namespace Nozomi.Base.BCL.Helpers.Crypto
{
    public static class Randomizer
    {
        // Adapted from
        // https://dotnetcodr.com/2016/10/05/generate-truly-random-cryptographic-keys-using-a-random-number-generator-in-net/
        public static string GenerateRandomCryptographicKey(int keyLength)
        {
            var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            var randomBytes = new byte[keyLength];
            rngCryptoServiceProvider.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }
    }
}