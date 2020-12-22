using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace LocalDatabase
{
    [CallbackBehavior(ConcurrencyMode = ConcurrencyMode.Reentrant, UseSynchronizationContext = false)]
    public class CallbackClient : IDatabaseCallback
    {

        Database db;
        IDatabaseService proxy = null;

        public CallbackClient(ref Database db)
        {
            this.db = db;
        }

        public void broadcastDeleteId(string id) {

            Console.WriteLine("Broadcasted delete id: {0}.\n", id);
            if (db.LogEntities.ContainsKey(id)) {
                db.LogEntities.Remove(id);
            }

        }

        public void broadcastUpdateId(string id) {

            Console.WriteLine("Broadcasted update id: {0}.", id);

            if (db.LogEntities.ContainsKey(id)) 
            {
                proxy.GetLogEntityById(id);
            }
        }

        public void broadcastAddLogEntity(Region region, string id)
        {
            Console.WriteLine($"Broadcasted Adding new Entity, region: {region.ToString()}, Id: {id}");

            if (db.RegioniOdInteresa.Contains(region))
            {
                if (!db.LogEntities.ContainsKey(id))
                {
                    LogEntity entity = proxy.GetLogEntityById(id);
                    db.LogEntities.Add(entity.Id,entity);
                }
            }

        }

        public IDatabaseService Proxy {
            get { return proxy; }
            set { proxy = value; }
        }

    }
}
