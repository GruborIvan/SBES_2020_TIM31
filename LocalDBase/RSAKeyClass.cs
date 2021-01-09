using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LocalDBase
{
    public class RSAKeyClass
    {
        internal static readonly string privateKey = "<RSAKeyValue><Modulus>68Rk+b6339zPb4cXhcsZkNxUYGcyVey7xBLozZ3sWAD8Y+yhI6s6lnKwkGBJ34k1IM0qV2c0FEhPsaIOsjsbsNePUzCDFoZtJNXt2LmxNeeQr4K76Ww9F/MNKpeVDhployax6jRzy6x4jCR4j0qBXlV4IerRs3FIkFgJrX3AY9U=</Modulus><Exponent>AQAB</Exponent><P>/dfu2sH64+PYPWPWJbrCnaLLzk4cEEX1CEXnjLf4Qlkx6M7uOsijUAZzqNPYAe4utp5fsGu/KlcHPzTsfWA5/w==</P><Q>7cUmAF4h21Uk8I3lkV6VxJfzsAY4UpM557wa25ym9o7+8uBge+4Plx3DEhvamMODpoY8LY1dQJN3lKLj181aKw==</Q><DP>Nr65HqizKS7cVfEQIDb0/fY8KhQibUgJHm2lEG4ktnpyDxmBu1/GCN47V2/IqDHsFSp2zJ+QLNt0DqelUSzNlQ==</DP><DQ>mVL5V2FcKci1Al4uPFim8VgfL1JDfZQs0e9tzlItJG8/njTsYt43tXXetc26X6osOfTz11gCW0L86J9Fl4J3cw==</DQ><InverseQ>/PaSg9iLeguuu7NRaRUYDNmJdrCxDku0zGUTB+wyn/q7KzQ2/m3D0sbE2WBv92N1U2SkChsXI6+19sfte+yVHw==</InverseQ><D>veC85fp4gf2Gvq6Q/jr1Cxq3hbB29IalLiOAhOC0EKgynJNdr6lelP4nPw+dTz9kn1c8y4mdgtw9+rSmm1pJUFZMEXGZNcr+MRX0ULTe9BRHgvc5KOmYInhGCwYLIYHPEzZgEjRxD7sU0XkeNYYOfdSQB/zejrsdFYRe2Fut6T0=</D></RSAKeyValue>";

        public static void DecryptData(string dataToDecr)
        {
            // read the encrypted bytes from the file   
            byte[] dataToDecrypt = Convert.FromBase64String(dataToDecr);

            // Create an array to store the decrypted data in it   
            byte[] decryptedData;
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                // Set the private key of the algorithm   
                rsa.FromXmlString(privateKey);
                decryptedData = rsa.Decrypt(dataToDecrypt, false);
            }

            Console.WriteLine(Convert.ToBase64String(decryptedData));
            new EncryptionLocalDB(decryptedData);

        }

    }
}
