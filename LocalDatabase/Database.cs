using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalDatabase
{
    public class Database
    {
        public Dictionary<string, LogEntity> LogEntities = new Dictionary<string, LogEntity>();

        public List<Region> RegioniOdInteresa = new List<Region>();

    }
}
