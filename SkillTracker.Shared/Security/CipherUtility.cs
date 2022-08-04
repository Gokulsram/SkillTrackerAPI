using System;
using System.Data.SqlClient;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace SkillTracker.Common.Helpers
{
    public static class CipherUtility
    {
        private static readonly string cipherPassKey = "AA_ENCRYPTION_KEY";
        private static readonly byte[] iv = new byte[16] { 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0 };
        public static string EncryptUserNamePasswordConnectionString(string connectionString)
        {
            var builder = new SqlConnectionStringBuilder(connectionString);
            builder.UserID = EncryptString(builder.UserID, cipherPassKey);
            builder.Password = EncryptString(builder.Password, cipherPassKey);
            return builder.ConnectionString;
        }
        public static string DecryptUserNamePasswordConnectionString(string connectionString)
        {
            var builder = new SqlConnectionStringBuilder(connectionString);
            builder.UserID = DecryptString(builder.UserID, cipherPassKey);
            builder.Password = DecryptString(builder.Password, cipherPassKey);
            return builder.ConnectionString;
        }
        public static string EncryptUserNamePasswordConnectionString(string connectionString, string passkey)
        {
            var builder = new SqlConnectionStringBuilder(connectionString);
            builder.UserID = EncryptString(builder.UserID, passkey);
            builder.Password = EncryptString(builder.Password, passkey);
            return builder.ConnectionString;
        }

        public static string DecryptUserNamePasswordConnectionString(string connectionString, string passkey)
        {
            var builder = new SqlConnectionStringBuilder(connectionString);
            builder.UserID = DecryptString(builder.UserID, passkey);
            builder.Password = DecryptString(builder.Password, passkey);
            return builder.ConnectionString;
        }
        public static string EncryptString(string plainText)
        {
            //return EncryptString(plainText, Environment.GetEnvironmentVariable(cipherPassKey));
            return EncryptString(plainText, "SkillTracker_Dev");
        }

        public static string DecryptString(string plainText)
        {
            //return DecryptString(plainText, Environment.GetEnvironmentVariable(cipherPassKey));
            return DecryptString(plainText, "SkillTracker_Dev");
        }

        public static string EncryptString(string plainText, string passkey)
        {
            string cipherText = string.Empty;
            using (SHA256 sHA256 = SHA256.Create())
            {
                byte[] key = sHA256.ComputeHash(Encoding.ASCII.GetBytes(passkey));
                using Aes encryptor = Aes.Create();
                encryptor.Mode = CipherMode.CBC;
                byte[] aesKey = new byte[32];
                Array.Copy(key, 0, aesKey, 0, 32);
                encryptor.Key = aesKey;
                encryptor.IV = iv;
                using MemoryStream memoryStream = new MemoryStream();
                using ICryptoTransform cryptoTransform = encryptor.CreateEncryptor();
                using CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Write);
                try
                {
                    byte[] plainBytes = Encoding.ASCII.GetBytes(plainText);
                    cryptoStream.Write(plainBytes, 0, plainBytes.Length);
                    cryptoStream.FlushFinalBlock();
                    byte[] cipherBytes = memoryStream.ToArray();
                    cipherText = Convert.ToBase64String(cipherBytes, 0, cipherBytes.Length);
                }
                finally
                {
                    memoryStream.Close();
                    cryptoStream.Close();
                }
            }
            return cipherText;
        }
        public static string DecryptString(string cipherText, string passkey)
        {
            string plainText = string.Empty;
            using (SHA256 sHA256 = SHA256.Create())
            {
                byte[] key = sHA256.ComputeHash(Encoding.ASCII.GetBytes(passkey));

                using Aes encryptor = Aes.Create();
                encryptor.Mode = CipherMode.CBC;
                byte[] aesKey = new byte[32];
                Array.Copy(key, 0, aesKey, 0, 32);
                encryptor.Key = aesKey;
                encryptor.IV = iv;
                using MemoryStream memoryStream = new MemoryStream();
                using ICryptoTransform aesDecryptor = encryptor.CreateDecryptor();
                using CryptoStream cryptoStream = new CryptoStream(memoryStream, aesDecryptor, CryptoStreamMode.Write);
                try
                {
                    byte[] cipherBytes = Convert.FromBase64String(cipherText);
                    cryptoStream.Write(cipherBytes, 0, cipherBytes.Length);
                    cryptoStream.FlushFinalBlock();
                    byte[] plainBytes = memoryStream.ToArray();
                    plainText = Encoding.UTF8.GetString(plainBytes, 0, plainBytes.Length);
                }
                finally
                {
                    memoryStream.Close();
                    cryptoStream.Close();
                }
            }
            return plainText;
        }
    }

}
