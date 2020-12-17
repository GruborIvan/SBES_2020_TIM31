using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace LocalDatabase
{
    public class WCFClient : ChannelFactory<IDatabaseService>, IDatabaseService, IDisposable
    {
        IDatabaseService factory;

        public WCFClient(NetTcpBinding binding, EndpointAddress address)
            : base(binding, address) {
            factory = this.CreateChannel();
            Console.WriteLine("PROVERA: Namestio sam!");
        }

        public string addLogEntity(LogEntitet entitet) {
            throw new NotImplementedException();
        }

        public float cityAverageConsumption(string grad) {
            throw new NotImplementedException();
        }

        public bool deleteLogEntity(string id) {
            throw new NotImplementedException();
        }

        public void Dispose() {
            if (factory != null) {
                factory = null;
            }

            this.Close();
        }

        public List<LogEntitet> readEntities(List<Region> regioni) {
            throw new NotImplementedException();
        }

        public float regionAverageConsumption(Region reg) {
            throw new NotImplementedException();
        }

        public void testservermessage(string message) {
            factory.testservermessage(message);
        }

        public LogEntitet updateConsumption(string id, int month, float consumption) {
            throw new NotImplementedException();
        }
    }
}
