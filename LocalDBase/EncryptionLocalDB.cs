﻿using Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LocalDBase
{
    public class EncryptionLocalDB
    {
        private static byte[] Key;
        private static readonly byte[] IV = Convert.FromBase64String("Aq0UThtJhjbuyWXtmZs1rw==");

        public EncryptionLocalDB()
        {

        }
         
        public EncryptionLocalDB(byte[] key1)
        {
            Key = key1;
        }


        public string decryptCall(byte[] cipherText)
        {
            return (Decrypt(cipherText));
        }
        public byte[] encryptCall(string plainText)
        {
            return (Encrypt(plainText));
        }
        public static string encryptFloat(ProsPotrosnja pp)
        {
            string s = JsonConvert.SerializeObject(pp);
            return Convert.ToBase64String(Encrypt(s));
        }
        public static ProsPotrosnja decryptFloat(string pp)
        {
            ProsPotrosnja le = JsonConvert.DeserializeObject<ProsPotrosnja>(Decrypt(Convert.FromBase64String(pp)));
            return le;
        }
        public static string encryptLogEntity(LogEntity le)
        {
            string s = JsonConvert.SerializeObject(le);
            return Convert.ToBase64String( Encrypt(s));
        }
        public static LogEntity decryptLogEntity(string logent)
        {
            LogEntity le = JsonConvert.DeserializeObject<LogEntity>(Decrypt(Convert.FromBase64String(logent)));
            return le;
        }
        public static string encryptListRegion(List<Region> regioni)
        {
            string s = JsonConvert.SerializeObject(regioni);
            return Convert.ToBase64String(Encrypt(s));
        }
        public static List<Region> decryptLogListRegion(string regioni)
        {
            List<Region> region = JsonConvert.DeserializeObject<List<Region>>(Decrypt(Convert.FromBase64String(regioni)));
            return region;
        }
        public static byte[] Encrypt(string plainText)
        {
            byte[] encrypted;

            using (AesManaged aes = new AesManaged())
            {
                aes.Mode = CipherMode.CBC;
                ICryptoTransform encryptor = aes.CreateEncryptor(Key, IV);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter sw = new StreamWriter(cs))
                            sw.Write(plainText);
                        encrypted = ms.ToArray();

                    }
                }
            }
            return encrypted;
        }
        public static string Decrypt(byte[] cipherText)
        {
            string plaintext = null;
            using (AesManaged aes = new AesManaged())
            {
                aes.Mode = CipherMode.CBC;
                ICryptoTransform decryptor = aes.CreateDecryptor(Key, IV);
                using (MemoryStream ms = new MemoryStream(cipherText))
                {
                    using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader reader = new StreamReader(cs))
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
