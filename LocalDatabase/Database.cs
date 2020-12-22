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
        private static Dictionary<string, LogEntity> repozitorijum = new Dictionary<string, LogEntity>();

        public List<Region> RegioniOdInteresa = new List<Region>();

        public Dictionary<string, LogEntity> EntityList {

            get { return repozitorijum; }
            set { repozitorijum = value; }
        }

    }
}
