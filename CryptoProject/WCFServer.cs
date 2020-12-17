using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoProject
{
    public class WCFServer : IDatabaseService
    {
        public string addLogEntity(LogEntitet entitet) {
            throw new NotImplementedException();
        }

        public float cityAverageConsumption(string grad) {
            throw new NotImplementedException();
        }

        public bool deleteLogEntity(string id) {
            throw new NotImplementedException();
        }

        public List<LogEntitet> readEntities(List<Region> regioni) {
            throw new NotImplementedException();
        }

        public float regionAverageConsumption(Region reg) {
            throw new NotImplementedException();
        }

        public void testservermessage(string message) {
            Console.WriteLine(message);
        }

        public LogEntitet updateConsumption(string id, int month, float consumption) {
            throw new NotImplementedException();
        }
    }
}
