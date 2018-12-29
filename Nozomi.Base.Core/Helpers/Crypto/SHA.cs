using System.Security.Cryptography;
using System.Text;

namespace Nozomi.Base.Core.Helpers.Crypto
{
    public static class SHA
    {
        public static string GenerateSHA256String(string inputString)
        {
            var sha256 = SHA256Managed.Create();
            var bytes = Encoding.UTF8.GetBytes(inputString);
            var hash = sha256.ComputeHash(bytes);
            return GetStringFromHash(hash);
        }

        public static string GenerateSHA512String(string inputString)
        {
            var sha512 = SHA512Managed.Create();
            var bytes = Encoding.UTF8.GetBytes(inputString);
            var hash = sha512.ComputeHash(bytes);
            return GetStringFromHash(hash);
        }

        private static string GetStringFromHash(byte[] hash)
        {
            var result = new StringBuilder();
            foreach (var t in hash)
            {
                result.Append(t.ToString("X2"));
            }
            return result.ToString();
        }
    }
}