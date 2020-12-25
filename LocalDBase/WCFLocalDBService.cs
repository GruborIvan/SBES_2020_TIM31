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
    class WCFLocalDBService : IDatabaseService
    {

        IDatabaseService proxy = null;
        public static List<IDatabaseCallback> klijenti = new List<IDatabaseCallback>();

        public WCFLocalDBService() {

            proxy = proxyCaller.proxy;

        }

        public string AddLogEntity(LogEntity entitet)
        {

            IDatabaseCallback callback = OperationContext.Current.GetCallbackChannel<IDatabaseCallback>();
            if (klijenti.Contains(callback) == false) {
                klijenti.Add(callback);
            }
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

            IDatabaseCallback callback = OperationContext.Current.GetCallbackChannel<IDatabaseCallback>();
            if (klijenti.Contains(callback) == false) {
                klijenti.Add(callback);
            }

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
            return proxy.UpdateConsumption(id, month, consumption);
        }
    }
}
