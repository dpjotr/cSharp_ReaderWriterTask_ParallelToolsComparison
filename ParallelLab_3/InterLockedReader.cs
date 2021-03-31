using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ParallelLab_3
{
    class InterLockedReader : Reader
    {
        internal InterLockedReader(int readerId) : base(readerId) { }

        internal override void AccessDB()
        {
            while (!Globals.db.finish)
            {
                if (0 == Interlocked.Exchange(ref Globals.db.readerUsingResource, 1))
                {
                    MyMessages.Add(Globals.db.buffer);
             
                    if (!Globals.db.finish) Interlocked.Exchange(ref Globals.db.writerUsingResource, 0);
                }
            }
        }
    }
}

