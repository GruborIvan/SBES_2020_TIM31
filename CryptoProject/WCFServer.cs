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
            return xh.AddEntity(entitet);
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
            List<LogEntitet> ret = new List<LogEntitet>();

            foreach(var item in regioni)
            {
                foreach(var predmet in xh.ReturnList())
                {
                    if (item.Equals(predmet.Region))
                    {
                        ret.Add(predmet);
                    }
                }
            }

            return ret;
        }

        public float regionAverageConsumption(Region reg) {
            float ret = 0, cons = 0;
            int n = 0, i = 0;

            foreach (LogEntitet item in xh.ReturnList())
            {
                if (item.Region.Equals(reg))
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

        public void testServerMessage(string message) {
            throw new NotImplementedException();
        }

        public LogEntitet updateConsumption(string id, int month, float consumption) {
            LogEntitet le = new LogEntitet();
            foreach (var element in xh.ReturnList())
            {
                if (element.Id.Equals(id))
                {
                    le = element;
                    break;
                }
            }
            le.Potrosnja[month] = consumption;
            xh.UpdateEntity(le);
            return le;

        }
    }
}
