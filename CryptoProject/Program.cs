using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
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

			host.Open();
			Console.WriteLine("WCFService is opened. Press <enter> to finish...");

            ///-----------------------------------------------------------------------
            
            NetTcpBinding bindingServer = new NetTcpBinding();
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
                Changes.ChangeList.Clear(); 
                Thread.Sleep(5000);
            }
        }
    }

}
