using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace LocalDatabase
{
    class Program
    {
        static void Main(string[] args) {

			NetTcpBinding binding = new NetTcpBinding();
			string address = "net.tcp://localhost:9999/wcfserver";

			using (WCFClient proxy = new WCFClient(binding, new EndpointAddress(new Uri(address)))) {
				proxy.testServerMessage("hello to server from client.");
			}

			Console.ReadLine();
		}
    }
}