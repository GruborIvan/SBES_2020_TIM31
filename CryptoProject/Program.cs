using CertificationManager;
using Common;
using SecurityManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CryptoProject
{
    class Program
    {
        static void Main(string[] args) {

            string centralDbCertCN = CertificationManager.Formatter.ParseName(WindowsIdentity.GetCurrent().Name);

            NetTcpBinding binding = new NetTcpBinding();

            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;
			string address = "net.tcp://localhost:9998/wcfserver";

			ServiceHost host = new ServiceHost(typeof(WCFServer));
			host.AddServiceEndpoint(typeof(IDatabaseService), binding, address);

            ServiceSecurityAuditBehavior newAudit = new ServiceSecurityAuditBehavior();
            newAudit.AuditLogLocation = AuditLogLocation.Application;
            newAudit.ServiceAuthorizationAuditLevel = AuditLevel.SuccessOrFailure;

            host.Description.Behaviors.Remove<ServiceSecurityAuditBehavior>();
            host.Description.Behaviors.Add(newAudit);

            host.Credentials.ClientCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.Custom;
            host.Credentials.ClientCertificate.Authentication.CustomCertificateValidator = new CentralDataBaseCertValidator();

            host.Credentials.ClientCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck;

            host.Credentials.ServiceCertificate.Certificate = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, centralDbCertCN);

            host.Open();
			Console.WriteLine("WCFService is opened. Press <enter> to finish...");

            ///-----------------------------------------------------------------------
            
            NetTcpBinding bindingServer = new NetTcpBinding();

            bindingServer.Security.Mode = SecurityMode.Transport;
            bindingServer.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;

            string addressServer = "net.tcp://localhost:7000/wcfBackup";

            WCFBackupClient proxy = new WCFBackupClient(bindingServer, new EndpointAddress(new Uri(addressServer)));

            //callbackclient.Proxy = proxy;
            ///-------------------------------------------------------------------------
            synchronizeBackup(proxy);

            Thread t = new Thread(() => threadWhileUpdate(proxy));
            t.Start();
            while (true)
            {
                //Console.WriteLine("1");
                Thread.Sleep(2500);
            }

        }
        static void threadWhileUpdate(WCFBackupClient proxy)
        {
            while (true)
            {
                //Console.WriteLine("2");
                proxy.sendChanges(Changes.ChangeList);
                try
                {
                    //AuditLoggingSystem.ChangesTupleListSent(WindowsIdentity.GetCurrent().Name, Changes.ChangeList.Count);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                Changes.ChangeList.Clear();
                Thread.Sleep(5000);
            }
        }

        static void synchronizeBackup(WCFBackupClient proxy) {

            XmlHandler xh = new XmlHandler();
            List<string> missingids = proxy.sendCentralDatabaseId(xh.ReturnList().Select(x => x.Id).ToList());

            List<LogEntity> missingentities = xh.ReturnList().FindAll(x => missingids.Contains(x.Id));
            proxy.sendMissingEntities(missingentities);

        }
    }

}
