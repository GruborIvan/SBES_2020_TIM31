using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace Backup
{
    public class WCFBackupServer : IBackupServer
    {
        XmlHandler xh = new XmlHandler();

        public bool sendChanges(List<Tuple<OperationCode, LogEntity>> promena)
        {
            foreach (Tuple<OperationCode, LogEntity> t in promena)
            {
                switch (t.Item1)
                {
                    case OperationCode.ADD:
                        xh.AddEntity(t.Item2);
                        break;
                    case OperationCode.DELETE:
                        xh.DeleteEntity(t.Item2.Id);
                        break;
                    case OperationCode.UPDATE:
                        xh.UpdateEntity(t.Item2);
                        break;
                }
            }

            return true;
        }

        public List<string> sendCentralDatabaseId(List<string> centraldatabaseids) {

            List<string> listaids = xh.ReturnList().Select(x => x.Id).ToList();

            return centraldatabaseids.FindAll(x => (listaids.Contains(x) == false));
        }

        public bool sendMissingEntities(List<LogEntity> logentities) {

            foreach (LogEntity entitet in logentities) {

                try {
                    xh.AddEntity(entitet);
                }
                catch (Exception ex) {
                    Console.WriteLine("Dodat entitet koji vec postoji u bekap bazi.");
                    return false;
                }
            }

            return true;
        }
    }
}
