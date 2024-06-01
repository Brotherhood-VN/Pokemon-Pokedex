using System.Security.Cryptography;
using System.Text;

namespace API.Helpers.Utilities
{
    public static class EncryptorUtility
    {
        // This constant string is used as a "salt" value for the PasswordDeriveBytes function calls.
        // This size of the IV (in bytes) must = (keysize / 8).  Default keysize is 256, so the IV must be
        // 32 bytes long.  Using a 16 character string here gives us 32 bytes when converted to a byte array.
        private static readonly byte[] initVectorBytes = Encoding.ASCII.GetBytes("ns93geji34it99u2");

        // This constant is used to determine the keysize of the encryption algorithm.
        private const int keysize = 256;

        public static string EncryptUserPassword(string plainPassword, string salt = "")
        {
            string encryptKey = AuthenticationKeys.ENCRYPT_PASSWORD_KEY + salt;
            return Encrypt(plainPassword, encryptKey);
        }

        public static string DescryptUserPassword(string encryptedPassword, string salt = "")
        {
            string encryptKey = AuthenticationKeys.ENCRYPT_PASSWORD_KEY + salt;
            return Decrypt(encryptedPassword, encryptKey);
        }

        private static string Encrypt(string plainText, string passPhrase, string salt = null)
        {
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            byte[] saltBytes = null;
            if (!string.IsNullOrEmpty(salt))
            {
                Encoding.ASCII.GetBytes(salt);
            }

            using PasswordDeriveBytes password = new(passPhrase, saltBytes);
            byte[] keyBytes = password.GetBytes(keysize / 8);
            using Aes aesAlg = Aes.Create();
            aesAlg.Mode = CipherMode.CBC;
            using ICryptoTransform encryptor = aesAlg.CreateEncryptor(keyBytes, initVectorBytes);
            using MemoryStream memoryStream = new();
            using CryptoStream cryptoStream = new(memoryStream, encryptor, CryptoStreamMode.Write);
            cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
            cryptoStream.FlushFinalBlock();
            byte[] cipherTextBytes = memoryStream.ToArray();
            return Convert.ToBase64String(cipherTextBytes);
        }

        private static string Decrypt(string cipherText, string passPhrase, string salt = null)
        {
            byte[] cipherTextBytes = Convert.FromBase64String(cipherText);
            byte[] saltBytes = null;
            if (!string.IsNullOrEmpty(salt))
            {
                Encoding.ASCII.GetBytes(salt);
            }
            using PasswordDeriveBytes password = new(passPhrase, saltBytes);
            byte[] keyBytes = password.GetBytes(keysize / 8);
            using Aes aesAlg = Aes.Create();
            aesAlg.Mode = CipherMode.CBC;
            using ICryptoTransform decryptor = aesAlg.CreateDecryptor(keyBytes, initVectorBytes);
            using MemoryStream memoryStream = new(cipherTextBytes);
            using CryptoStream cryptoStream = new(memoryStream, decryptor, CryptoStreamMode.Read);
            byte[] plainTextBytes = new byte[cipherTextBytes.Length];
            int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
            return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
        }
    }

    public static class AuthenticationKeys
    {
        public const string ENCRYPT_PASSWORD_KEY = @"Z-)P}$W}8G.uHU5J";
        public const string ENCRYPT_EXTERNAL_LOGIN_KEY = @"sn_'\3*Mp@pFA?H$";
        public const string ENCRYPT_TOKEN = @"-mR(kjJ3:SKpYUKL";
    }
}