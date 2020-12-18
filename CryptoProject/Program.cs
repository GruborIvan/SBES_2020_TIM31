using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace CryptoProject
{
    class Program
    {

        static void Main(string[] args) {

			NetTcpBinding binding = new NetTcpBinding();
			string address = "net.tcp://localhost:9999/wcfserver";

			ServiceHost host = new ServiceHost(typeof(WCFServer));
			host.AddServiceEndpoint(typeof(IDatabaseService), binding, address);

			host.Open();
			Console.WriteLine("WCFService is opened. Press <enter> to finish...");
			Console.ReadLine();

            LogEntitet le = new LogEntitet("0",  Region.BACKA, "AAAA", 1231);
            LogEntitet le3 = new LogEntitet("3", Region.BACKA, "DDDD", 1231);
            LogEntitet le1 = new LogEntitet("1", Region.BANAT, "BBBB", 1241);
            LogEntitet le2 = new LogEntitet("2", Region.BANAT, "CCCC", 1241);
            XmlHandler xh = new XmlHandler();

            xh.AddEntity(le);
            xh.AddEntity(le1);
            
            xh.DeleteEntity("0");

            xh.AddEntity(le2);
            xh.AddEntity(le3);

            xh.DeleteEntity("1");
            le3.Potrosnja[10] = 1231;
            le3.Grad = "DEQS";
            xh.UpdateEntity(le3);

            foreach (LogEntitet item in xh.ReturnList())
            {
                Console.WriteLine("id: " + item.Id+ "   grad: " + item.Grad);
            }
            
            Console.WriteLine("Create Update Delete 3 entiteta kreirana 2 Entiteta sa updatovanim potrosnjama 3. obrisan ");

            Console.ReadLine();

        }
    }
}
