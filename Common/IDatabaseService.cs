using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Common
{

    [ServiceContract]
    public interface IDatabaseService
    {

        [OperationContract]
        void testservermessage(string message);
        [OperationContract]
        List<LogEntitet> readEntities(List<Region> regioni);
        [OperationContract]
        float cityAverageConsumption(string grad);
        [OperationContract]
        float regionAverageConsumption(Region reg);
        [OperationContract]
        string addLogEntity(LogEntitet entitet);
        [OperationContract]
        LogEntitet updateConsumption(string id, int month, float consumption);
        [OperationContract]
        bool deleteLogEntity(string id);

    }
}
