using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Common.Helpers
{
    public class CryptHelper
    {
        private static readonly byte[] IV = null; // byte array :)
        private static readonly int BlockSize = 128;
        protected static readonly string Key = "" ; //encryption key :)
        public static string Encrypt(string mess_To_Encrypt)
        {
            string to_send = "";
            byte[] bytes = Encoding.Unicode.GetBytes(mess_To_Encrypt);
            SymmetricAlgorithm crypt = Aes.Create();
            HashAlgorithm hash = MD5.Create();
            crypt.BlockSize = BlockSize;
            crypt.Key = hash.ComputeHash(Encoding.Unicode.GetBytes(Key));
            crypt.IV = IV;

            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (CryptoStream cryptoStream =
                   new CryptoStream(memoryStream, crypt.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cryptoStream.Write(bytes, 0, bytes.Length);
                }

                to_send = Convert.ToBase64String(memoryStream.ToArray());
            }
            return to_send;
        }

        public static dynamic Decrypt(string crypted_Mess)
        {
            string encrypted_Mess = "";
            byte[] bytes = Convert.FromBase64String(crypted_Mess.Replace("<EOF>", ""));
            SymmetricAlgorithm crypt = Aes.Create();
            HashAlgorithm hash = MD5.Create();
            crypt.Key = hash.ComputeHash(Encoding.Unicode.GetBytes(Key));
            crypt.IV = IV;
            using (MemoryStream memoryStream = new MemoryStream(bytes))
            {
                using (CryptoStream cryptoStream =
                   new CryptoStream(memoryStream, crypt.CreateDecryptor(), CryptoStreamMode.Read))
                {
                    byte[] decryptedBytes = new byte[bytes.Length];
                    cryptoStream.Read(decryptedBytes, 0, decryptedBytes.Length);
                    encrypted_Mess = Encoding.Unicode.GetString(decryptedBytes);
                }
            }
            return encrypted_Mess;
        }
    }
}
