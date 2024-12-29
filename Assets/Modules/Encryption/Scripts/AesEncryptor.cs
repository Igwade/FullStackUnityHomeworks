using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Modules.Ecryption
{
    public static class AesEncryptor
    {
        public static string Encrypt(string input, string password, byte[] salt)
        {
            byte[] convertedInput = Encoding.UTF8.GetBytes(input);
            byte[] encryptedInput = Encrypt(convertedInput, password, salt);

            return Convert.ToBase64String(encryptedInput);
        }

        public static byte[] Encrypt(byte[] input, string password, byte[] salt)
        {
            using Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(password, salt);

            using MemoryStream ms = new MemoryStream();
            using Aes aes = Aes.Create();

            aes.Key = pdb.GetBytes(aes.KeySize / 8);
            aes.IV = pdb.GetBytes(aes.BlockSize / 8);

            using CryptoStream cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write);

            cs.Write(input, 0, input.Length);
            cs.Close();

            return ms.ToArray();
        }

        public static string Decrypt(string input, string password, byte[] salt)
        {
            byte[] convertedInput = Convert.FromBase64String(input);
            byte[] decryptedInput = Decrypt(convertedInput, password, salt);

            return Encoding.UTF8.GetString(decryptedInput);
        }

        public static byte[] Decrypt(byte[] input, string password, byte[] salt)
        {
            using Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(password, salt);
            using MemoryStream ms = new MemoryStream();
            using Aes aes = Aes.Create();

            aes.Key = pdb.GetBytes(aes.KeySize / 8);
            aes.IV = pdb.GetBytes(aes.BlockSize / 8);

            using CryptoStream cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write);

            cs.Write(input, 0, input.Length);
            cs.Close();

            return ms.ToArray();
        }
    }
}