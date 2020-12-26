using System.Collections.Generic;
using System.ServiceModel;

namespace Common
{

    [ServiceContract(CallbackContract = typeof(IDatabaseCallback))]
    public interface IDatabaseService
    {

        [OperationContract]
        void testServerMessage(string message);

        [OperationContract]
        List<LogEntity> GetEntitiesForRegions(List<Region> regioni);

        [OperationContract]
        float GetAverageConsumptionForCity(string city);

        [OperationContract]
        float GetAverageConsumptionForRegion(Region reg);

        [OperationContract]
        float GetAverageConsumptionForRegionList(string reg);

        [OperationContract]
        string AddLogEntity(LogEntity entitet);

        [OperationContract]
        LogEntity UpdateConsumption(string id, int month, float consumption);

        [OperationContract]
        bool DeleteLogEntity(string id);

        [OperationContract]
        LogEntity GetLogEntityById(string id);

        [OperationContract]
        List<LogEntity> GetEntitiesForRegionsString(string regioni);

        [OperationContract]
        string GetAverageConsumptionForCityRetStr(string city);

        [OperationContract]
        string GetAverageConsumptionForRegionRetStr(string reg);
    }
}
