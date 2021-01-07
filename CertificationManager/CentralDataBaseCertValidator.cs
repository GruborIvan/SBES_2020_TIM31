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
        public override void Validate(X509Certificate2 localDBcertificate)
        {
            //Debugger.Launch();
            X509Certificate2 cdbCert = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine,
                Formatter.ParseName(WindowsIdentity.GetCurrent().Name));

            Console.WriteLine("Issuer Local DataBase: " + localDBcertificate.Issuer);
            Console.WriteLine("SubjectName Central DataBase: " + cdbCert.Subject);

            if (!localDBcertificate.Issuer.Equals(cdbCert.Subject))
            {
                throw new Exception("Issuer i Subject se ne poklapaju!");
            }
        }
    }
}
