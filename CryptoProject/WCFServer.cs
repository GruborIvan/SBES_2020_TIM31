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
        public void testservermessage(string message) {
            Console.WriteLine(message);
        }
    }
}
