using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalDatabase
{
    public class CallbackClient : IDatabaseCallback
    {
        public void broadcastDeleteId(string id) {
            Console.WriteLine("Broadcasted delete id: {0}.", id);
        }

        public void broadcastUpdateId(string id) {
            Console.WriteLine("Broadcasted update id: {0}.", id);

        }
    }
}
