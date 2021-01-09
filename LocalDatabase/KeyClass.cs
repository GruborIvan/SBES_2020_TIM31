using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class KeyClass
    {
        internal readonly static byte[] Key = Convert.FromBase64String("AsISxq9OwdZag1163OJqwovXfSWG98m+sPjVwJecfe4=");

        internal readonly static byte[] IV = Convert.FromBase64String("Aq0UThtJhjbuyWXtmZs1rw==");

        internal static readonly string publicKey = "<RSAKeyValue><Modulus>68Rk+b6339zPb4cXhcsZkNxUYGcyVey7xBLozZ3sWAD8Y+yhI6s6lnKwkGBJ34k1IM0qV2c0FEhPsaIOsjsbsNePUzCDFoZtJNXt2LmxNeeQr4K76Ww9F/MNKpeVDhployax6jRzy6x4jCR4j0qBXlV4IerRs3FIkFgJrX3AY9U=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";

        public static byte[] encrKey()
        {
             // Convert the text to an array of bytes   
            UnicodeEncoding byteConverter = new UnicodeEncoding();
            byte[] dataToEncrypt = Key;

            // Create a byte array to store the encrypted data in it   
            byte[] encryptedData;
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                // Set the rsa pulic key   
                rsa.FromXmlString(publicKey);

                // Encrypt the data and store it in the encyptedData Array   
                encryptedData = rsa.Encrypt(dataToEncrypt, false);
            }

            return encryptedData;
        }
    }
}
