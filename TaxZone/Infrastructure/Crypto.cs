using System.Text;
using System.Security.Cryptography;

namespace TaxZone.Infrastructure
{
    public static class Crypto
    {
        public static string Encrypt(string value)
        {
            if (string.IsNullOrEmpty(value))
                return value;

            byte[] data = Encoding.UTF8.GetBytes(value);

            byte[] encrypted = ProtectedData.Protect(
                data,
                null,
                DataProtectionScope.CurrentUser);

            return Convert.ToBase64String(encrypted);
        }

        public static string Decrypt(string value)
        {
            if (string.IsNullOrEmpty(value))
                return value;

            byte[] encrypted = Convert.FromBase64String(value);

            byte[] decrypted = ProtectedData.Unprotect(
                encrypted,
                null,
                DataProtectionScope.CurrentUser);

            return Encoding.UTF8.GetString(decrypted);
        }
    }
}
