using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Selectors;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace CertificationManager
{
    public class CentralDataBaseCertValidator : X509CertificateValidator
    {
         //u parametru dobijam client cert
        public override void Validate(X509Certificate2 localDBcertificate)
        {
            Debugger.Launch();
            X509Certificate2 cdbCert = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine,
                Formatter.ParseName(WindowsIdentity.GetCurrent().Name));

            if (!localDBcertificate.Issuer.Equals("CN=CentralDbCA"))
            {
                Console.WriteLine("Certificate nisu izdala ista CA!");
                //throw new Exception("EX: Certificate nisu izdala ista CA!!!!!!!!");
            }
        }
    }
}
