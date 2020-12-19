using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace CryptoProject
{
    public class WCFServer : IDatabaseService
    {
        XmlHandler xh = new XmlHandler();
        private static Dictionary<string, IIdentity> klijenti = new Dictionary<string, IIdentity>();

        public string addLogEntity(LogEntitet entitet) {

            //dictionary za klijent id vezu 
            IIdentity client = ServiceSecurityContext.Current.PrimaryIdentity;
            if (client == null)
                return null;
            klijenti.Add(entitet.Id, client);
            /////////////////////////////////
            
            return xh.AddEntity(entitet);
        }

        public float cityAverageConsumption(string grad) {
            /////////////////////////////////////////get() klijenta koji trazi pristup
            IIdentity client = ServiceSecurityContext.Current.PrimaryIdentity;
            ////////////////////////////////////////
            float ret = 0,cons = 0;
            int n = 0,i = 0;

            foreach(LogEntitet item in xh.ReturnList())
            {/////////////////////////////////////////ako klijentu nije dostupna dotican item -> continue
             //   if (!klijenti[item.Id].Equals(client))
             //       continue;
            /////////////////////////////////////////
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
            return (ret / n); //Ne!
        }

        public bool deleteLogEntity(string id)
        {//////////////////////////////////////Ako klijentu nije dostupan doticni item -> return
            IIdentity client = ServiceSecurityContext.Current.PrimaryIdentity;
            //if (!klijenti[id].Equals(client))
            //    return false;
        ///////////////////////////////////////
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
            //////////////////////////////////////
            IIdentity client = ServiceSecurityContext.Current.PrimaryIdentity;
            ///////////////////////////////////////
            
            List<LogEntitet> ret = new List<LogEntitet>();

            foreach(var item in regioni) // Why?
            {
                
                foreach(var predmet in xh.ReturnList())
                {////////////////////////////////////////////
                    //if (!klijenti[predmet.Id].Equals(client))
                    //    continue;
                ////////////////////////////////////////////
                    if (item.Equals(predmet.Region))
                    {
                        ret.Add(predmet);
                    }
                }
            }

            return ret;
        }

        public float regionAverageConsumption(Region reg) {
            //////////////////////////////////////
            IIdentity client = ServiceSecurityContext.Current.PrimaryIdentity;
            ///////////////////////////////////////
            
            float ret = 0, cons = 0;
            int n = 0, i = 0;

            foreach (LogEntitet item in xh.ReturnList())
            {
                ///////////////////////////////////////
                //if (!klijenti[item.Id].Equals(client)) //Problem ovde!
                //    continue;
                //////////////////////////////////////
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
            return (ret / n); // Ne!
        }

        public void testServerMessage(string message) {
            throw new NotImplementedException();
        }

        public LogEntitet updateConsumption(string id, int month, float consumption) {
            //////////////////////////////////////
            IIdentity client = ServiceSecurityContext.Current.PrimaryIdentity;
            ///////////////////////////////////////

            LogEntitet le = new LogEntitet();
            foreach (var element in xh.ReturnList())
            {
            ///////////////////////////////////////
               // if (!klijenti[element.Id].Equals(client))
               //     continue;
            //////////////////////////////////////
              
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
