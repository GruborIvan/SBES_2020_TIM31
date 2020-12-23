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
        public static Dictionary<string, LogEntity> LogEntities = new Dictionary<string, LogEntity>();

        public static List<Region> RegioniOdInteresa = new List<Region>();

        public Dictionary<string, LogEntity> EntityList {

            get { return LogEntities; }
            set { LogEntities = value; }
        }

        public List<Region> InterestRegions {

            get { return RegioniOdInteresa; }
            set { RegioniOdInteresa = value; }
        }

    }
}
