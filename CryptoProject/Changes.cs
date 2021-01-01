using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace CryptoProject
{
    public class Changes
    {
        private static readonly object myLock = new object();
        private static List<Tuple<OperationCode, LogEntity>> changeList = new List<Tuple<OperationCode, LogEntity>>();

        public static List<Tuple<OperationCode, LogEntity>> ChangeList
        {
            get { lock (myLock) { return changeList; } }
            set { lock (myLock) { changeList = value; } }
        }
    }
}
