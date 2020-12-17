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

        private static List<LogEntitet> repozitorijum = new List<LogEntitet>();

        public List<LogEntitet> EntityList {

            get { return repozitorijum; }
            set { repozitorijum = value; }
        }

    }
}
