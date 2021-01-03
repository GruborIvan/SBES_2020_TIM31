using Common;
using SecurityManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CryptoProject
{
    class Program
    {
        static void Main(string[] args) {

			NetTcpBinding binding = new NetTcpBinding();
			string address = "net.tcp://localhost:9998/wcfserver";

			ServiceHost host = new ServiceHost(typeof(WCFServer));
			host.AddServiceEndpoint(typeof(IDatabaseService), binding, address);

            ServiceSecurityAuditBehavior newAudit = new ServiceSecurityAuditBehavior();
            newAudit.AuditLogLocation = AuditLogLocation.Application;
            newAudit.ServiceAuthorizationAuditLevel = AuditLevel.SuccessOrFailure;

            host.Description.Behaviors.Remove<ServiceSecurityAuditBehavior>();
            host.Description.Behaviors.Add(newAudit);

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

            Thread t = new Thread(() => threadWhileUpdate(proxy));
            t.Start();
            while (true)
            {
                Console.WriteLine("1");
                Thread.Sleep(2500);
            }
            Console.ReadLine();

        }
        static void threadWhileUpdate(WCFBackupClient proxy)
        {
            while (true)
            {
                Console.WriteLine("2");
                proxy.sendChanges(Changes.ChangeList);
                //AuditLoggingSystem.ChangesTupleListSent(WindowsIdentity.GetCurrent().Name, Changes.ChangeList.Count);
                Changes.ChangeList.Clear();
                Thread.Sleep(5000);
            }
        }
    }

}
