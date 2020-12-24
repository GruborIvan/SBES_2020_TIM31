using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalDBase
{
    class WCFLocalDBService : IDatabaseService
    {

        IDatabaseService proxy = null;

        public WCFLocalDBService() {

            proxy = proxyCaller.proxy;

        }

        public string AddLogEntity(LogEntity entitet)
        {
            throw new NotImplementedException();
        }

        public bool DeleteLogEntity(string id)
        {
            throw new NotImplementedException();
        }

        public float GetAverageConsumptionForCity(string city)
        {
            throw new NotImplementedException();
        }

        public float GetAverageConsumptionForRegion(Region reg)
        {
            throw new NotImplementedException();
        }

        public List<LogEntity> GetEntitiesForRegions(List<Region> regioni)
        {
            throw new NotImplementedException();
        }

        public LogEntity GetLogEntityById(string id)
        {
            throw new NotImplementedException();
        }

        public void testServerMessage(string message)
        {
            proxy.testServerMessage(message);
        }

        public LogEntity UpdateConsumption(string id, int month, float consumption)
        {
            throw new NotImplementedException();
        }
    }
}
