using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace BasicExtensions
{
    public static class EncryptionExtensions
    {
        private static byte[] GetRandomBytes()
        {
            try
            {
                var data = new byte[16];
                using (var rgn = new RNGCryptoServiceProvider())
                {
                    rgn.GetBytes(data);
                    return data;
                }
            }
            catch
            {
                return new byte[] { 202, 28, 223, 164, 177, 123, 212, 124, 132, 124, 52, 12, 6, 12, 45, 124 };
            }
        }
        public static string EncryptText(this string text, string key = "")
        {
            var saltStringBytes = GetRandomBytes();
            var ivStringBytes = GetRandomBytes();
            var plainTextBytes = Encoding.UTF8.GetBytes(text);
            using (var password = new Rfc2898DeriveBytes(key, saltStringBytes, 1000))
            {
                var keyBytes = password.GetBytes(128 / 8);
                using (var symmetricKey = new RijndaelManaged())
                {
                    symmetricKey.BlockSize = 128;
                    symmetricKey.Mode = CipherMode.CBC;
                    symmetricKey.Padding = PaddingMode.PKCS7;
                    using (var encryptor = symmetricKey.CreateEncryptor(keyBytes, ivStringBytes))
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                            {
                                cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                                cryptoStream.FlushFinalBlock();
                                var cipherTextBytes = saltStringBytes;
                                cipherTextBytes = cipherTextBytes.Concat(ivStringBytes).ToArray();
                                cipherTextBytes = cipherTextBytes.Concat(memoryStream.ToArray()).ToArray();
                                memoryStream.Close();
                                cryptoStream.Close();
                                return Convert.ToBase64String(cipherTextBytes);
                            }
                        }
                    }
                }
            }
        }
        public static string DecryptText(this string text, string key = "")
        {
            var cipherTextBytesWithSaltAndIv = Convert.FromBase64String(text);
            var saltStringBytes = cipherTextBytesWithSaltAndIv.Take(128 / 8).ToArray();
            var ivStringBytes = cipherTextBytesWithSaltAndIv.Skip(128 / 8).Take(128 / 8).ToArray();
            var cipherTextBytes = cipherTextBytesWithSaltAndIv.Skip((128 / 8) * 2).Take(cipherTextBytesWithSaltAndIv.Length - ((128 / 8) * 2)).ToArray();

            using (var password = new Rfc2898DeriveBytes(key, saltStringBytes, 1000))
            {
                var keyBytes = password.GetBytes(128 / 8);
                using (var symmetricKey = new RijndaelManaged())
                {
                    symmetricKey.BlockSize = 128;
                    symmetricKey.Mode = CipherMode.CBC;
                    symmetricKey.Padding = PaddingMode.PKCS7;
                    using (var decryptor = symmetricKey.CreateDecryptor(keyBytes, ivStringBytes))
                    {
                        using (var memoryStream = new MemoryStream(cipherTextBytes))
                        {
                            using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                            {
                                var plainTextBytes = new byte[cipherTextBytes.Length];
                                var decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                                memoryStream.Close();
                                cryptoStream.Close();
                                return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
                            }
                        }
                    }
                }
            }
        }
        public static string CreateMD5(this string input)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }
    }
}
