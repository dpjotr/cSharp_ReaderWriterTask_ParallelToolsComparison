using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParallelLab_3
{
    class SemReader : Reader
    {
        internal SemReader(int readerId) : base(readerId) { }

        internal override void AccessDB()
        {
            while (!Globals.db.finish)
            {
                if (!Globals.db.bEmpty)
                {
                    Globals.sem.WaitOne();
                    {
                        if (!Globals.db.bEmpty)
                        {
                            Globals.db.bEmpty = true;
                            MyMessages.Add(Globals.db.buffer);
                        }
                    }
                    Globals.sem.Release();
                }
            }
        }
    }
}

