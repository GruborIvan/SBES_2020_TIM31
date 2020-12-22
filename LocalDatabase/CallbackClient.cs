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
        IDatabaseService proxy = null;

        public void broadcastDeleteId(string id) {

            Database database = new Database();
            Console.WriteLine("Broadcasted delete id: {0}.\n", id);
            if (database.EntityList.ContainsKey(id)) {
                database.EntityList.Remove(id);
            }

        }

        public void broadcastUpdateId(string id) {

            Console.WriteLine("Broadcasted update id: {0}.", id);
            Database database = new Database();

            if (database.EntityList.ContainsKey(id)) {
                proxy.GetLogEntityById(id);
            }

        }

        public void broadcastAddLogEntity(Region region, string id)
        {
            Console.WriteLine($"Broadcasted Adding new Entity, region: {region.ToString()}, Id: {id}");

            Database db = new Database();
            if (!db.EntityList.ContainsKey(id))
            {
                proxy.GetLogEntityById(id);
            }
        }

        public IDatabaseService Proxy {
            get { return proxy; }
            set { proxy = value; }
        }

    }
}
