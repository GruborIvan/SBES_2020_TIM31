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
        private static Dictionary<string, LogEntitet> repozitorijum = new Dictionary<string, LogEntitet>();

        public Dictionary<string, LogEntitet> EntityList {

            get { return repozitorijum; }
            set { repozitorijum = value; }
        }

    }
}
