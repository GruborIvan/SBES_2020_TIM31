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
        public string AddLogEntity(LogEntity entitet)
        {
            return AddLogEntity(entitet);
        }

        public bool DeleteLogEntity(string id)
        {
            return DeleteLogEntity(id);
        }

        public float GetAverageConsumptionForCity(string city)
        {
            return GetAverageConsumptionForCity(city);
        }

        public float GetAverageConsumptionForRegion(Region reg)
        {
            return GetAverageConsumptionForRegion(reg);
        }

        public List<LogEntity> GetEntitiesForRegions(List<Region> regioni)
        {
            return GetEntitiesForRegions(regioni);
        }

        public LogEntity GetLogEntityById(string id)
        {
            return GetLogEntityById(id);
        }

        public void testServerMessage(string message)
        {
            testServerMessage(message);
        }

        public LogEntity UpdateConsumption(string id, int month, float consumption)
        {
            return UpdateConsumption(id, month, consumption);
        }
    }
}
