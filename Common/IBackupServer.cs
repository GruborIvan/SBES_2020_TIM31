using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [ServiceContract]
    public interface IBackupServer
    {

        [OperationContract]
        List<string> sendCentralDatabaseId(List<string> centraldatabaseids);
        [OperationContract]
        bool sendMissingEntities(List<LogEntity> logentities);
        [OperationContract]
        bool sendChanges(List<Tuple<OperationCode, LogEntity>> promena);

    }
}
