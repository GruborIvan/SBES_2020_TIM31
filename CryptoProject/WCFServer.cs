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
        XmlHandler xh = new XmlHandler();
        public string addLogEntity(LogEntitet entitet) {

            throw new NotImplementedException();
        }

        public float cityAverageConsumption(string grad) {
            float ret = 0,cons = 0;
            int n = 0,i = 0;

            foreach(LogEntitet item in xh.ReturnList())
            {
                if (item.Grad.Equals(grad))
                {
                    foreach (float f in item.Potrosnja)
                    {
                        cons += item.Potrosnja[i];
                        i++;
                    }
                    ret += cons;
                    cons = 0;
                    i = 0;
                    n++;
                }
            }
            return (ret / n);
        }

        public bool deleteLogEntity(string id) {
            try
            {
             return xh.DeleteEntity(id);

            }catch(Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public List<LogEntitet> readEntities(List<Region> regioni) {
            try { return xh.ReturnList(); }
            catch (Exception e) { Console.WriteLine(e); }
            return null;
        }

        public float regionAverageConsumption(Region reg) {
            throw new NotImplementedException();
        }

        public void testServerMessage(string message) {
            throw new NotImplementedException();
        }

        public LogEntitet updateConsumption(string id, int month, float consumption) {
            throw new NotImplementedException();
        }
    }
}
