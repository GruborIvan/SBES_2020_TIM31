using CertificationManager;
using Client;
using Common;
using CryptoProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace LocalDBase
{
    class Program
    {

        static void Main(string[] args)
        {
            //MARCETA RADI
            string centralDbCertCN = "CentralDbCA";
            NetTcpBinding bindingServer = new NetTcpBinding();
            string addressServer = "net.tcp://localhost:9998/wcfserver";

            bindingServer.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;
            X509Certificate2 centralDbCert = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, centralDbCertCN);
            EndpointAddress address = new EndpointAddress(new Uri(addressServer), new X509CertificateEndpointIdentity(centralDbCert));

            CallbackLocalDatabase callbackclient = new CallbackLocalDatabase();
            WCFLocalDB proxy = new WCFLocalDB(callbackclient, bindingServer, address);
            //callbackclient.Proxy = proxy;

            proxyCaller callerbase = new proxyCaller();
            callerbase.ProxySetter = proxy;

            ////////////////////////////

            int port = callerbase.openNewEndpoint();

            string baseName = "baza" + port.ToString() + ".xml";

            XmlHandler xh = new XmlHandler(baseName);

            Console.WriteLine("WCFService is opened. Press <enter> to finish...");

            Console.ReadLine();
        }
        
    }
}
