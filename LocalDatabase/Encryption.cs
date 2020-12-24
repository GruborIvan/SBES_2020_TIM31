using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LocalDatabase
{
    public class Encryption
    {
        public Encryption()
        {

        }
        public string decryptCall(byte[] cipherText, byte[] key, byte[] IV)
        {
            return (Decrypt(cipherText, key, IV));
        }
        public byte[] encryptCall(string plainText, byte[] key, byte[] IV)
        {
            return (Encrypt(plainText, key, IV));
        }

        public static byte[] Encrypt(string plainText, byte[] key, byte[] IV)
        {
            byte[] encrypted;

            using (AesManaged aes = new AesManaged())
            {
                ICryptoTransform encryptor = aes.CreateEncryptor(key, IV);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        using(StreamWriter sw = new StreamWriter(cs))
                        sw.Write(plainText);
                        encrypted = ms.ToArray();
                        
                    }
                }
            }
            return encrypted;
        }
        public static string Decrypt(byte[] cipherText, byte[] key, byte[] IV)
        {
            string plaintext = null;
            using(AesManaged aes = new AesManaged())
            {
                ICryptoTransform decryptor = aes.CreateDecryptor(key, IV);
                using (MemoryStream ms = new MemoryStream(cipherText))
                {
                    using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    {
                        using(StreamReader reader = new StreamReader(cs))
                        {
                            plaintext = reader.ReadToEnd();
                        }
                    }
                }
            }

            return plaintext;
        }
    }
}
