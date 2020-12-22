using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public interface IDatabaseCallback
    {

        [OperationContract]
        void broadcastUpdateId(string id);

        [OperationContract]
        void broadcastDeleteId(string id);

        [OperationContract]
        void broadcastAddLogEntity(Region region,string id);
    }
}
