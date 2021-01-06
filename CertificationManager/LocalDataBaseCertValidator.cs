using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Selectors;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace CertificationManager
{
    public class LocalDataBaseCertValidator : X509CertificateValidator
    {
        //U parametru dobijan service cert
        public override void Validate(X509Certificate2 centralDBcertificate)
        {
            //Debugger.Launch();
            //subjectname = centraldatabase, issuer = CentralDbCA
            if(!centralDBcertificate.Subject.Equals(centralDBcertificate.Issuer))
            {
                Console.WriteLine("CentralDB cert subject name != Issuer");
            }
                
        }
    }
}
