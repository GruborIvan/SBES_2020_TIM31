using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace Backup
{
    class Program
    {
        static void Main(string[] args)
        {
            NetTcpBinding binding = new NetTcpBinding();
            string address = "net.tcp://localhost:7000/wcfBackup";

            ServiceHost host = new ServiceHost(typeof(WCFBackupServer));
            host.AddServiceEndpoint(typeof(IBackupServer), binding, address);

            host.Open();
            Console.WriteLine("WCFBackup is opened. Press <enter> to finish...");
            Console.ReadLine();

        }
    }
}
