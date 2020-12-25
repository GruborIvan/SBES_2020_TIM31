using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace LocalDBase
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Reentrant)]
    public class WCFLocalDBService : IDatabaseService
    {

        IDatabaseService proxy = null;

        public WCFLocalDBService() {

            proxy = proxyCaller.proxy;

        }

        public string AddLogEntity(LogEntity entitet)
        {
            return proxy.AddLogEntity(entitet);
        }

        public bool DeleteLogEntity(string id)
        {
            return proxy.DeleteLogEntity(id);
        }

        public float GetAverageConsumptionForCity(string city)
        {
            return proxy.GetAverageConsumptionForCity(city);
        }

        public float GetAverageConsumptionForRegion(Region reg)
        {
            return proxy.GetAverageConsumptionForRegion(reg);
        }

        public List<LogEntity> GetEntitiesForRegions(List<Region> regioni)
        {
            return proxy.GetEntitiesForRegions(regioni);
        }

        public LogEntity GetLogEntityById(string id)
        {
            return proxy.GetLogEntityById(id);
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
