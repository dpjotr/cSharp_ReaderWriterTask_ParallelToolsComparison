using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParallelLab_3
{
    class Database
    {
        public object locker;
        public string buffer;
        public bool bEmpty;
        public bool finish;
        public int writerUsingResource;
        public int readerUsingResource;

        public Database()
        {
            locker = new object();
            buffer = "";
            bEmpty = true;
            finish = false;
        }
    }
}
