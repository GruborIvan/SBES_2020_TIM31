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
    public class LocalDataBaseCertValidator : X509CertificateValidator
    {
        public override void Validate(X509Certificate2 centralDBcertificate)
        {
            X509Certificate2 ldbCert = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine,
                Formatter.ParseName(WindowsIdentity.GetCurrent().Name));

            //Debugger.Launch();
            Console.WriteLine("Issuer: " + centralDBcertificate.Issuer);
            Console.WriteLine("Subject: " + centralDBcertificate.Subject);

            if(!centralDBcertificate.Subject.Equals(ldbCert.Issuer))
            {
                throw new Exception("CentralDB SubjectName nije isti kao LocalDB Issuer!");
            }
                
        }
    }
}
